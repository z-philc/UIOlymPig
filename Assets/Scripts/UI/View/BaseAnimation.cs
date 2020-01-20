using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseAnimation : MonoBehaviour
{
    public virtual void ShowViewAnimation(Action callBack)
    {
        if (callBack != null)
        {
            callBack();
        }
    }
    public virtual void HideViewAnimation(Action callBack)
    {
        if (callBack != null)
        {
            callBack();
        }
    }
}
