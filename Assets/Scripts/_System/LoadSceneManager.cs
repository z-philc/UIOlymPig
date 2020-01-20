using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LoadSceneData
{
    public string sceneName;
    public Action<object> callBack;
}
public class LoadSceneManager : SingletonMono<LoadSceneManager>
{
    public void OnSceneLoad(string name, Action<object> callBack)
    {
        StopCoroutine("LoadScene");
        StartCoroutine("LoadScene", new LoadSceneData
        {
            sceneName = name,
            callBack = callBack
        });
    }
    IEnumerator LoadScene(LoadSceneData data)
    {
        LoadingView loadingView = null;
        ViewManager.Instance.SwitchView(ViewIndex.LoadingView, null, () =>
        {
            loadingView = ViewManager.Instance.currentView as LoadingView;
        });
        yield return new WaitUntil(()=>loadingView != null);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(data.sceneName, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        { 
            loadingView.progress.fillAmount = asyncLoad.progress;
            yield return null;
        }

        if (data.callBack != null)
        {
            data.callBack("Hi");
        }
        if (ViewManager.Instance.currentView.viewIndex == ViewIndex.LoadingView)
        {
            ViewManager.Instance.SwitchView(ViewIndex.EmptyView);
        }
    }
}
