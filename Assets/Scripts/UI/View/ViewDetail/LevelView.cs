using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelView : BaseView
{
    public RectTransform content;
    public Button backButton;

    [SerializeField] private List<LevelMap> levelMapItems;
    private bool isFirstInit = false;

    public override void Setup(ViewParam param, Action callBack = null)
    {
        print("LevelView setup ");
        if (isFirstInit == false)
        {
            int levelCompleted = PlayerPrefs.GetInt(Global.Data_LevelCompleted);
          
            foreach (var e in levelMapItems)
            {
                e.Setup(levelCompleted);
            }
            isFirstInit = true;
        }
        base.Setup(param, callBack);
    }

    private void OnEnable()
    {
        backButton.onClick.AddListener(OnClickBackButton);
    }

    public override void OnShow(Action callBack)
    {
        //baseAnimation.GetComponent<RectTransform>().DOAnchorPosX(0f, 0.5f);
        base.OnShow(callBack);
    }

    public void ActiveNewLevel(int index)
    {
        levelMapItems[index - 1].ActiveHighlight();
        levelMapItems[index].Unlock();
    }

    void OnClickBackButton()
    {
        ViewManager.Instance.SwitchView(ViewIndex.HomeView);
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveListener(OnClickBackButton);
    }
}
