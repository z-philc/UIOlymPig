using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMono<UIManager>
{
    public GameObject collectionGoalLayout;

    public int collectionGoalBaseWidth = 125;

    // UI.Text that shows how many moves are left
    public Text movesLeftText;

    // reference to three-star score meter
    public ScoreMeter scoreMeter;

    public Text currentLevelText;

    public GameObject movesCounter;

    public Timer timer;

    public Button pauseButton;

    private void OnEnable()
    {
        pauseButton.onClick.AddListener(OnPauseGame);
    }

    public void SetupCollectionGoalLayout(CollectionGoal[] collectionGoals, GameObject goalLayout, int spacingWidth)
    {
        if (goalLayout != null && collectionGoals != null && collectionGoals.Length != 0)
        {
            RectTransform rectXform = goalLayout.GetComponent<RectTransform>();
            rectXform.sizeDelta = new Vector2(collectionGoals.Length * spacingWidth, rectXform.sizeDelta.y);

            CollectionGoalPanel[] panels = goalLayout.GetComponentsInChildren<CollectionGoalPanel>();

            for (int i = 0; i < panels.Length; i++)
            {
                if (i < collectionGoals.Length && collectionGoals[i] != null)
                {
                    panels[i].gameObject.SetActive(true);
                    panels[i].collectionGoal = collectionGoals[i];
                    panels[i].SetupPanel();
                }
                else
                {
                    panels[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void SetupCollectionGoalLayout(CollectionGoal[] collectionGoals)
    {
        SetupCollectionGoalLayout(collectionGoals, collectionGoalLayout, collectionGoalBaseWidth);
    }

    public void UpdateCollectionGoalLayout(GameObject goalLayout)
    {
        if (goalLayout != null)
        {
            CollectionGoalPanel[] panels = goalLayout.GetComponentsInChildren<CollectionGoalPanel>();

            if (panels != null && panels.Length != 0)
            {
                foreach (CollectionGoalPanel panel in panels)
                {
                    if (panel != null && panel.isActiveAndEnabled)
                    {
                        panel.UpdatePanel();
                    }
                }
            }
        }
    }

    public void UpdateCollectionGoalLayout()
    {
        UpdateCollectionGoalLayout(collectionGoalLayout);
    }

    public void EnableTimer(bool state)
    {
        if (timer != null)
        {
            timer.gameObject.SetActive(state);
            timer.timeLeftText.text = LevelGoal.Instance.timeLeft.ToString();
        }
    }

    public void EnableMovesCounter(bool state)
    {
        if (movesCounter != null)
        {
            movesCounter.SetActive(state);
        }
    }

    public void EnableCollectionGoalLayout(bool state)
    {
        if (collectionGoalLayout != null)
        {
            collectionGoalLayout.SetActive(state);
        }
    }

    public void OnPauseGame()
    {
        DialogManager.Instance.ShowDialog(DialogIndex.PauseDialog);
    }

    private void OnDisable()
    {
        pauseButton.onClick.RemoveListener(OnPauseGame);
    }
}
