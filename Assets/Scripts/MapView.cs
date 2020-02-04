using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapView : MonoBehaviour
{

    public Button btnBack;
    public Button btnNext;
    [SerializeField] GameObject currentMap;
    [SerializeField] String mapName = "Map";
    private int mapNumber;

    void Start()
    {
        StartCoroutine(FindCurrentMap());
    }

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

        //print(currentMap.transform.GetChild(0));
        //print("back");
    }

    void BtnNextClick()
    {
        //print("next");
    }

    void GetMapInMapList(bool backClick = true, bool nextClick = true)
    {

    }

    IEnumerator FindCurrentMap()
    {
        //Instantiate(Resources.Load("Maps/Map1", typeof(GameObject))) ;

        /* If not save data, default of mapNumber is 1 */
        //if (saved)
        //{
        //    mapNumber = saved
        //}
        mapNumber = 1;
        mapName += mapNumber.ToString();
        print(gameObject.name);
        string path = "Maps/" + mapName;
        currentMap = Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;

        currentMap.transform.SetParent(GameObject.FindWithTag("Respawn").transform);

        // Instantiate an map object
        yield return new WaitForEndOfFrame();
    }
}
