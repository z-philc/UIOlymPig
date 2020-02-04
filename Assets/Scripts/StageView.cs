using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageView : MonoBehaviour
{
    public Button btnBack;
    public Button btnNext;
    [SerializeField] GameObject currentStage;
    [SerializeField] List<GameObject> stageList;
    [SerializeField] String stageName = "Stage";
    private int stageNumber;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FindCurrentStage()
    {
        // get current stage
        yield return new WaitForEndOfFrame();
    }

    IEnumerator ShowStageOfList()
    {
        yield return new WaitForEndOfFrame();
    }
}
