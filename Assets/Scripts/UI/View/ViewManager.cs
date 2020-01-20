using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ViewManager : SingletonMono<ViewManager>
{
    public Dictionary<ViewIndex, BaseView> dicView = new Dictionary<ViewIndex, BaseView>();

    public BaseView currentView;
    public BaseView previousView;

    // Use this for initialization
    void Awake()
    {
        foreach (ViewIndex e in ViewConfig.viewIndices)
        {
            string s = e.ToString();
            GameObject goView = Instantiate(Resources.Load("View/" + s, typeof(GameObject))) as GameObject;
            goView.transform.SetParent(transform);
            goView.transform.localScale = Vector3.one;

            RectTransform rect = goView.GetComponent<RectTransform>();
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;
            BaseView baseView = goView.GetComponent<BaseView>();

            dicView.Add(baseView.viewIndex, baseView);
            goView.SetActive(false);
        }
        currentView = null;
        SwitchView(ViewIndex.LoadingView);
    }
    public void SwitchView(ViewIndex e,ViewParam param=null, Action callBack=null)
    {
        if (currentView != null)
        {
            currentView.OnHide(() =>{
                ShowNewView(e, param, callBack);
            });
        }
        else
        {
            ShowNewView(e, param, callBack);
        }
    }
    private void ShowNewView(ViewIndex e, ViewParam param = null, Action callBack = null)
    {
        currentView = dicView[e];
        currentView.Setup(param, callBack);
    }
}
