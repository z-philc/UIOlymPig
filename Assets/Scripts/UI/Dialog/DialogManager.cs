using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogManager : SingletonMono<DialogManager>
{
    private Dictionary<DialogIndex, BaseDialog> dicDialog = new Dictionary<DialogIndex, BaseDialog>();
    public List<BaseDialog> lsShow = new List<BaseDialog>();

    void Start()
    {
        foreach (DialogIndex e in DialogConfig.dialogIndices)
        {
            string s = e.ToString();
            GameObject dialogObject = Instantiate(Resources.Load("Dialog/" + s, typeof(GameObject))) as GameObject;
            dialogObject.transform.SetParent(transform, false);
            RectTransform rect = GetComponent<RectTransform>();
            rect.offsetMax = Vector3.zero;
            rect.offsetMin = Vector3.zero;
            BaseDialog dialog = dialogObject.GetComponent<BaseDialog>();
            dicDialog.Add(dialog.dialogIndex,dialog);
            dialogObject.SetActive(false);
        }
    }

    public void ShowDialog(DialogIndex index, DialogParam param = null, Action callBack = null)
    {
        BaseDialog baseDialog = dicDialog[index];
        baseDialog.ShowDialog(param, callBack);
        lsShow.Add(baseDialog);
    }

    public void HideDialog(BaseDialog dialog, Action callBack = null)
    {
        dialog.HideDialog(callBack);
        lsShow.Remove(dialog);
        Time.timeScale = 1;
    }

    public void HideDialog(DialogIndex index, Action callBack = null)
    {
        if (lsShow.Contains(dicDialog[index]))
        {
             dicDialog[index].HideDialog(callBack);
            lsShow.Remove(dicDialog[index]);
        }           
        Time.timeScale = 1;
    }

    public void HideAllDialog(Action callBack)
    {
        if (lsShow.Count == 0) return;
        for (int i = 0; i < lsShow.Count; i++)
        {
            lsShow[i].HideDialog(null);
        }
        lsShow[lsShow.Count - 1].HideDialog(callBack);
        lsShow.Clear();
        Time.timeScale = 1;
    }
}
