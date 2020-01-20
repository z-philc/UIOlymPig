using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseDialog : MonoBehaviour
{
    public DialogIndex dialogIndex;
    public BaseDialogAnimation baseDialogAnimation;
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        baseDialogAnimation = GetComponentInChildren<BaseDialogAnimation>();
    }

    public virtual void OnSetup(DialogParam param=null)
    {

    }

    public void ShowDialog(DialogParam param = null, Action callBack = null)
    {
        gameObject.SetActive(true);
        rect.SetAsLastSibling();
        OnSetup(param);
        baseDialogAnimation.ShowAnimation(() =>
        {
            OnShow();
            if (callBack != null)
            {
                callBack();
            }
        });
    }

    public virtual void OnShow()
    {

    }

    public void HideDialog(Action callBack = null)
    {
        baseDialogAnimation.HideAnimation(() =>
        {
            OnHide();
            
            if (callBack != null)
            {
                callBack();
            }
        });
    }

    public virtual void OnHide()
    {
        gameObject.SetActive(false);
    }
}
