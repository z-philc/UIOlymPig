using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapView : MonoBehaviour
{

    public Button btnBack;
    public Button btnNext;
    [SerializeField] GameObject currentMap;

    //public event Action

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        btnBack.onClick.AddListener(BtnBackClick);
        btnNext.onClick.AddListener(BtnNextClick);
    }

    void BtnBackClick()
    {
        currentMap = GameObject.Find("Scroll");
        print(currentMap.transform.GetChild(0));
        print("back");
    }

    void BtnNextClick()
    {
        print("next");
    }
}
