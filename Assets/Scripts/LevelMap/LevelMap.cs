using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMap : MonoBehaviour
{
    public int level;
    public GameObject highlight;

    private Button playButton;
    public GameObject lockImage;
    public bool isUnlocked;
    public bool isCompleted;

    private void OnEnable()
    {
        playButton = gameObject.GetComponent<Button>();
        playButton.onClick.AddListener(OnPlay);
    }

    private void OnPlay()
    {
        if (!isUnlocked) return;
        Global.currentLevel = this.level;
        LoadSceneManager.Instance.OnSceneLoad("Ingame", null);
    }

    public void Setup(int levelCompleted)
    {
        if (levelCompleted == 0)
        {
            if (level == 1)
            {
                isUnlocked = true;
                lockImage.gameObject.SetActive(false);
                highlight.gameObject.SetActive(false);
                return;
            }
        }

        if (level <= levelCompleted)
        {
            isUnlocked = true;
            lockImage.gameObject.SetActive(false);
            this.isCompleted = true;
            highlight.gameObject.SetActive(true);
        }

        if (level == levelCompleted + 1)
        {
            isUnlocked = true;
            lockImage.gameObject.SetActive(false);
        }
    }

    public void Unlock()
    {
        isUnlocked = true;
        lockImage.gameObject.SetActive(false);
    }

    public void ActiveHighlight()
    {
        highlight.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(OnPlay);
    }
}
