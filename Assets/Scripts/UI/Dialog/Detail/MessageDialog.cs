using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MessageDialog : BaseDialog
{
    [SerializeField] private Image messageImage;
    [SerializeField] private Text messageText;

    [SerializeField] private Sprite loseIcon;
    [SerializeField] private Sprite winIcon;
    [SerializeField] private Sprite goalIcon;
    [SerializeField] private Sprite timerIcon;
    [SerializeField] private Sprite movesIcon;
    [SerializeField] private Sprite goalCompleteIcon;
    [SerializeField] private Sprite goalFailedIcon;

    [SerializeField] private Image goalImage;
    [SerializeField] private Text goalText;
    [SerializeField] private Text levelText;

    [SerializeField] private Button startButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private Text replayButtonText;
    [SerializeField] private Image bGPanel;
    MessageDialogParam cf = null;

    private void OnEnable()
    {
        startButton.onClick.AddListener(OnStartGameClick);
        replayButton.onClick.AddListener(OnReplayClick);
        homeButton.onClick.AddListener(OnHomeClick);
    }

    public override void OnSetup(DialogParam param = null)
    {
        ResetDialog();

        cf = (MessageDialogParam)param;
        if (cf != null)
        {
            if (cf.messageType == DialogMessageType.Start)
            {
                levelText.gameObject.SetActive(true);
                levelText.text = "level " + Global.currentLevel; 
                startButton.gameObject.SetActive(true);
                homeButton.gameObject.SetActive(false);
                replayButton.gameObject.SetActive(false);

                ShowScoreMessage(cf.scoreGoal);
                if (cf.levelCounter == LevelCounter.Timer)
                {
                    ShowTimedGoal(cf.timeGoal);
                }
                else
                {
                    ShowMovesGoal(cf.moveGoal);
                }
            }
            else if (cf.messageType == DialogMessageType.Win)
            {
                levelText.gameObject.SetActive(false);
                startButton.gameObject.SetActive(false);
                homeButton.gameObject.SetActive(true);
                replayButton.gameObject.SetActive(true);
                replayButtonText.text = "next";
                ShowWinMessage();
                if (ScoreManager.Instance != null)
                {
                    string scoreStr = "you scored\n" + ScoreManager.Instance.CurrentScore.ToString() + " points!";
                    ShowGoalCaption(scoreStr);
                    ShowGoalImage(goalCompleteIcon);
                    SoundManager.Instance?.PlayWinSound();
                }
                Global.currentLevel++;
            }
            else if (cf.messageType == DialogMessageType.Lose)
            {
                levelText.gameObject.SetActive(false);
                startButton.gameObject.SetActive(false);
                homeButton.gameObject.SetActive(true);
                replayButton.gameObject.SetActive(true);
                replayButtonText.text = "replay";
                ShowLoseMessage();
                string caption = "";

                if (GameManager.Instance.LevelGoal.levelCounter == LevelCounter.Timer)
                    caption = "Out of time!";
                else caption = "Out of moves!";

                ShowGoalCaption(caption);
                ShowGoalImage(goalFailedIcon);
                SoundManager.Instance?.PlayLoseSound();
            }
        }
        base.OnSetup(param);
    }

    private void ResetDialog()
    {
        Vector2 originPosY = new Vector2(0f, 1500f);
        baseDialogAnimation.GetComponent<RectTransform>().anchoredPosition = originPosY;
        bGPanel.DOFade(0f, 0f);
    }

    public override void OnShow()
    {
        baseDialogAnimation.GetComponent<RectTransform>().DOAnchorPosY(0, 1f).OnStart(() =>
        {
            bGPanel.DOFade(0.85f, 0.5f);
        });
        base.OnShow();
    }

    public override void OnHide()
    {
        baseDialogAnimation.GetComponent<RectTransform>().DOAnchorPosY(-1500f, 0.8f).OnStart(() =>
        {
            bGPanel.DOFade(0, 0.85f);
        }).OnComplete(() =>
        {
            base.OnHide();
        });
    }

    public void ShowMessage(Sprite sprite = null, string message = "")
    {
        if (messageImage != null)
        {
            messageImage.sprite = sprite;
        }

        if (messageText != null)
        {
            messageText.text = message;
        }
    }

    public void ShowGoal(string caption = "", Sprite icon = null)
    {
        if (caption != "")
        {
            ShowGoalCaption(caption);
        }

        if (icon != null)
        {
            ShowGoalImage(icon);
        }
    }

    public void ShowGoalCaption(string caption = "")
    {
        if (goalText != null)
        {
            goalText.text = caption;
        }
    }

    public void ShowGoalImage(Sprite icon = null)
    {
        if (goalImage != null)
        {
            goalImage.gameObject.SetActive(true);
            goalImage.sprite = icon;
        }

        if (icon == null)
        {
            goalImage.gameObject.SetActive(false);
        }
    }

    public void ShowScoreMessage(int scoreGoal)
    {
        string message = "score goal \n" + scoreGoal.ToString();
        ShowMessage(goalIcon, message);
    }

    public void ShowWinMessage()
    {
        ShowMessage(winIcon, "level\ncomplete");
    }

    public void ShowLoseMessage()
    {
        ShowMessage(loseIcon, "level\nfailed");
    }

    public void ShowTimedGoal(int time)
    {
        string caption = time.ToString() + " seconds";
        ShowGoal(caption, timerIcon);
    }

    public void ShowMovesGoal(int moves)
    {
        string caption = moves.ToString() + " moves";
        ShowGoal(caption, movesIcon);
    }

    void OnStartGameClick()
    {
        GameManager.Instance.BeginGame();
        DialogManager.Instance.HideDialog(this);

        bool useTimer = (GameManager.Instance.LevelGoal.levelCounter == LevelCounter.Timer);
        UIManager.Instance.EnableTimer(useTimer);
        UIManager.Instance.EnableMovesCounter(!useTimer);
    }

    void OnReplayClick()
    {
        GameManager.Instance.ReloadScene();
    }

    void OnHomeClick()
    {
        DialogManager.Instance.HideAllDialog(null);
        LoadSceneManager.Instance.OnSceneLoad("Buffer", (obj) => ViewManager.Instance.SwitchView(ViewIndex.HomeView));
    }

    private void OnDisable()
    {
        homeButton.onClick.RemoveListener(OnReplayClick);
        startButton.onClick.RemoveListener(OnStartGameClick);
        replayButton.onClick.RemoveListener(OnHomeClick);
    }

    /*May be need feature later, show items what need collection
     * public void ShowCollectionGoal(bool state = true)
    {
        if (collectionGoalLayout != null)
        {
            collectionGoalLayout.SetActive(state);
        }

        if (state)
        {
            ShowGoal("", collectIcon);
        }
        Debug.LogError("Show Collection Goal Message");
    }
    */
}
