using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LevelGoal))]
public class GameManager : SingletonMono<GameManager>
{
    public int lastLevelComplete;
    public World currentWorld;
    public int currentLevel = 0;

    [SerializeField] private Board board;

    // is the player read to play?
    bool isReadyToBegin = false;
    
    bool isGameOver = false;

    public bool IsGameOver { get { return isGameOver; } set { isGameOver = value; } }

    bool isWinner = false;

    // are we ready to load/reload a new level?
    bool isReadyToReload = false;

    [SerializeField] private LevelGoal levelGoal;

    [SerializeField] private LevelGoalCollected levelGoalCollected;

    public LevelGoal LevelGoal { get { return levelGoal; } }

    void ConfigureLevel(int levelIndex)
    {
        print("configure level: " + levelIndex);
        if (currentWorld == null)
        {
            Debug.LogError("GAMEMANAGER SetupLevelData: missing world...");
            return;
        }

        if (levelIndex >= currentWorld.levels.Length)
        {
            print("error");
            //print(currentWorld.levels.Length);
            //Debug.LogError("GAMEMANAGER SetupLevelData: invalid level index...");
            //return;
        }

        if (board == null)
        {
            Debug.LogError("GAMEMANAGER SetupLevelData: missing Board...");
            return;
        }

        // reference to the Level ScriptableObject (just for readability)
        Level levelConfig = currentWorld.levels[levelIndex];

        board.width = levelConfig.width;
        board.height = levelConfig.height;
        board.startingTiles = levelConfig.startingTiles;
        board.startingGamePieces = levelConfig.startingGamePieces;
        board.startingBlockers = levelConfig.startingBlockers;
        board.gamePiecePrefabs = levelConfig.gamePiecePrefabs;
        board.chanceForCollectible = levelConfig.chanceForCollectible;

        // we need to create a new Collection Goal array by instantiating the prefabs
        List<CollectionGoal> goals = new List<CollectionGoal>();
        foreach (CollectionGoal g in levelConfig.collectionGoals)
        {
            CollectionGoal instance = Instantiate(g, transform);
            goals.Add(instance);
        }

        // we can only assign the array of instances to the 
        levelGoalCollected.collectionGoals = goals.ToArray();
        levelGoalCollected.scoreGoals = levelConfig.scoreGoals;
        levelGoalCollected.movesLeft = levelConfig.movesLeft;
        levelGoalCollected.timeLeft = levelConfig.timeLeft;
        levelGoalCollected.maxTime = levelConfig.timeLeft;
        levelGoalCollected.levelCounter = levelConfig.levelCounter;
        board.BoardInit();
    }

    void Start()
    {
        currentLevel = Global.currentLevel;
        //print(currentLevel);
        //print(currentWorld.name);
        //print(currentWorld.ToString());
        ConfigureLevel(Global.currentLevel - 1);

        if (UIManager.Instance != null)
        {
            // position ScoreStar horizontally
            if (UIManager.Instance.scoreMeter != null)
            {
                UIManager.Instance.scoreMeter.SetupStars(levelGoal);
            }

            if (levelGoalCollected != null)
            {
                UIManager.Instance.EnableCollectionGoalLayout(true);
                UIManager.Instance.SetupCollectionGoalLayout(levelGoalCollected.collectionGoals);
            }
            else
            {
                UIManager.Instance.EnableCollectionGoalLayout(false);
            }

            UIManager.Instance.EnableTimer(false);
            UIManager.Instance.EnableMovesCounter(false);
            UIManager.Instance.currentLevelText.text = "Level " + Global.currentLevel;
        }

        // update the moves left UI
        levelGoal.movesLeft++;
        UpdateMoves();

        // start the main game loop
        StartCoroutine("ExecuteGameLoop");
    }

    // update the Text component that shows our moves left
    public void UpdateMoves()
    {
        // if the LevelGoal is not timed (e.g. LevelGoalScored)...
        if (levelGoal.levelCounter == LevelCounter.Moves)
        {
            // decrement a move
            levelGoal.movesLeft--;

            // update the UI
            if (UIManager.Instance != null && UIManager.Instance.movesLeftText != null)
            {
                UIManager.Instance.movesLeftText.text = levelGoal.movesLeft.ToString();
            }
        }
    }

    // this is the main coroutine for the Game, that determines are basic beginning/middle/end
    // each stage of the game must complete before we advance to the next stage
    // add as many stages here as necessary
    IEnumerator ExecuteGameLoop()
    {
        yield return StartCoroutine("StartGameRoutine");
        yield return StartCoroutine("PlayGameRoutine");

        // wait for board to refill
        yield return StartCoroutine("WaitForBoardRoutine", 0.5f);

        yield return StartCoroutine("EndGameRoutine");
    }

    // switches ready to begin status to true
    public void BeginGame()
    {
        isReadyToBegin = true;
    }

    // coroutine for the level introduction
    IEnumerator StartGameRoutine()
    {
        int maxGoall = levelGoal.scoreGoals.Length - 1;
        MessageDialogParam param = new MessageDialogParam
        {
            messageType = DialogMessageType.Start,
            levelCounter = levelGoal.levelCounter,
            scoreGoal = levelGoal.scoreGoals[maxGoall],
            timeGoal = levelGoal.timeLeft,
            moveGoal = levelGoal.movesLeft,
        };
        DialogManager.Instance.ShowDialog(DialogIndex.MessageDialog, param, null);

        // wait until the player is ready
        while (!isReadyToBegin)
        {
            yield return null;
        }

        // wait half a second
        yield return new WaitForSeconds(0.5f);

        // setup the Board
        if (board != null)
        {
            board.boardSetup.SetupBoard();
        }
    }

    // coroutine for game play
    IEnumerator PlayGameRoutine()
    {
        // if level is timed, start the timer
        if (levelGoal.levelCounter == LevelCounter.Timer)
        {
            levelGoal.StartCountdown();
        }
        // while the end game condition is not true, we keep playing
        // just keep waiting one frame and checking for game conditions
        while (!isGameOver)
        {
            isGameOver = levelGoal.IsGameOver();

            isWinner = levelGoal.IsWinner();

            // wait one frame
            yield return null;
        }
    }

    IEnumerator WaitForBoardRoutine(float delay = 0f)
    {
        if (levelGoal.levelCounter == LevelCounter.Timer && UIManager.Instance != null
            && UIManager.Instance.timer != null)
        {
            UIManager.Instance.timer.paused = true;
        }

        if (board != null)
        {
            // this accounts for the swapTime delay in the Board's SwitchTilesRoutine BEFORE ClearAndRefillRoutine is invoked
            yield return new WaitForSeconds(board.swapTime);

            // wait while the Board is refilling
            while (board.isRefilling)
            {
                yield return null;
            }
        }

        // extra delay before we go to the EndGameRoutine
        yield return new WaitForSeconds(delay);
    }

    // coroutine for the end of the level
    IEnumerator EndGameRoutine()
    {
        // set ready to reload to false to give the player time to read the screen
        isReadyToReload = false;

        if (isWinner)
        {
            ShowWinScreen();
        }
        else
        {
            ShowLoseScreen();
        }

        yield return new WaitForSeconds(1f);

        while (!isReadyToReload)
        {
            yield return null;
        }

        DialogManager.Instance.HideDialog(DialogIndex.MessageDialog);

        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    void ShowWinScreen()
    {
        PlayerPrefs.SetInt(Global.Data_LevelCompleted, currentLevel);
        MessageDialogParam param = new MessageDialogParam
        {
            messageType = DialogMessageType.Win
        };
        DialogManager.Instance.ShowDialog(DialogIndex.MessageDialog, param, null);

        LevelView lvView = (LevelView)ViewManager.Instance.dicView[ViewIndex.LevelView];
        lvView.ActiveNewLevel(currentLevel);
    }

    void ShowLoseScreen()
    {
        MessageDialogParam param = new MessageDialogParam
        {
            messageType = DialogMessageType.Lose
        };

        DialogManager.Instance.ShowDialog(DialogIndex.MessageDialog, param, null);
    }

    // use this to acknowledge that the player is ready to reload
    public void ReloadScene()
    {
        isReadyToReload = true;
    }

    // score points and play a sound
    public void ScorePoints(int chainReaction = 1, int totalItemsErase = 0)
    {
        if (ScoreManager.Instance != null)
        {
            int point = 0;
            switch (totalItemsErase)
            {
                case 3:
                    point = 30;
                    break;
                case 4:
                    point = totalItemsErase * 15;
                    break;
                case 5:
                    point = totalItemsErase * 16;
                    break;
                case 6:
                    point = totalItemsErase * 17;
                    break;
                case 7:
                    point = totalItemsErase * 18;
                    break;
                default:
                    point = totalItemsErase * 18;
                    break;
            }
            if (chainReaction != 1)
            {
                point *= 2;
            }
            //Debug.LogError("TotalItemsErase:" + totalItemsErase + "  " + "ChainRect: " + chainReaction + " = " + point);
            // score points
            ScoreManager.Instance.AddScore(point);

            // update the scoreStars in the Level Goal component
            levelGoal.UpdateScoreStars(ScoreManager.Instance.CurrentScore);

            if (UIManager.Instance != null && UIManager.Instance.scoreMeter != null)
            {
                UIManager.Instance.scoreMeter.UpdateScoreMeter(ScoreManager.Instance.CurrentScore,
                    levelGoal.scoreStars);
            }
        }
    }

    public void SoundAtItemsClear(GamePiece piece)
    {
        if (SoundManager.Instance != null && piece.clearSound != null)
        {
            SoundManager.Instance.PlayClipAtPoint(piece.clearSound, Vector3.zero, SoundManager.Instance.fxVolume);
        }
    }

    public void AddTime(int timeValue)
    {
        if (levelGoal.levelCounter == LevelCounter.Timer)
        {
            levelGoal.AddTime(timeValue);
        }
    }

    public void UpdateCollectionGoals(GamePiece pieceToCheck)
    {
        if (levelGoalCollected != null)
        {
            levelGoalCollected.UpdateGoals(pieceToCheck);
        }
    }
}
