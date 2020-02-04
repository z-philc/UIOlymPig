using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeView : MonoBehaviour
{

    public Button btnPlay;
    public Button btnSetting;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        btnPlay.onClick.AddListener(BtnPlayClick);
        btnSetting.onClick.AddListener(BtnSettingClick);
    }

    void BtnPlayClick()
    {
        // load Stage view
        print("play game");
    }

    void BtnSettingClick()
    {
        print("setting");
    }
}
