using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootLoaderManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
#if UNITY_STANDALONE
        Screen.SetResolution(1080, 1920, true);
        //Screen.orientation = ScreenOrientation.Portrait;
#endif
    }

    private void Start()
    {
        LoadSceneManager.Instance.OnSceneLoad("Buffer", (obj) =>
        {
            ViewManager.Instance.SwitchView(ViewIndex.HomeView);
        });
    }
}
