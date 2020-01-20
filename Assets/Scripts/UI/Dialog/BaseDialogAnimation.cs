using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseDialogAnimation : MonoBehaviour {

	public virtual void ShowAnimation(Action callBack)
    {
        if (callBack != null)
        {
            callBack();
        }
    }
    public virtual void HideAnimation(Action callBack)
    {
        if (callBack != null)
        {
            callBack();
        }
    }
}
