using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BaseView : MonoBehaviour
{
    public ViewIndex viewIndex;
    
    public BaseAnimation baseAnimation;

    private void Reset()
    {
        BaseAnimation bA = gameObject.GetComponentInChildren<BaseAnimation>();
        if (bA == null)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(transform);
            go.name = "BaseAnimation";
            go.AddComponent<BaseAnimation>();
            go.AddComponent<RectTransform>();
        }
    }
    private void Awake()
    {
        baseAnimation = gameObject.GetComponentInChildren<BaseAnimation>();
    }
    public virtual void Setup(ViewParam param, Action callBack = null)
    {
        OnShow(() =>
        {
            if (callBack != null)
            {
                callBack();
            }
        });
    }

    public virtual void OnHide(Action callBack)
    {
        baseAnimation.HideViewAnimation(() =>
        {
            gameObject.SetActive(false);
            if (callBack != null)
            {
                callBack();
            }
        });

    }

    public virtual void OnShow(Action callBack)
    {
        gameObject.SetActive(true);
        baseAnimation.ShowViewAnimation(callBack);
    }
}
