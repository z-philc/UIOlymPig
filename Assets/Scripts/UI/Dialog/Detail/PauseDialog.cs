using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class PauseDialog : BaseDialog
{
    [SerializeField] private Button homeButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Image bgPanel;
    private void OnEnable()
    {
        homeButton.onClick.AddListener(OnHomeButtonClick);
        continueButton.onClick.AddListener(OnContinueButtonClick);
        musicToggle.onValueChanged.AddListener(OnClickMusicToggle);
        soundToggle.onValueChanged.AddListener(OnClickSoundToggle);
    }

    public override void OnSetup(DialogParam param = null)
    {
        baseDialogAnimation.GetComponent<RectTransform>().localScale = Vector3.zero;
        base.OnSetup(param);
    }

    public override void OnShow()
    {
        bgPanel.DOFade(0.8f, 0f);

        if (PlayerPrefs.GetInt(Global.Data_Sound) == 0)
            soundToggle.isOn = true;
        else
            soundToggle.isOn = false;
        SoundManager.Instance.isOnSound = soundToggle.isOn;

        if (PlayerPrefs.GetInt(Global.Data_Music) == 0)
            musicToggle.isOn = true;
        else
            musicToggle.isOn = false;
        SoundManager.Instance.PlayBackGroundMusic(musicToggle.isOn);

        baseDialogAnimation.GetComponent<RectTransform>().DOScale(1, 0.5f).OnStart(()=> {
            bgPanel.DOFade(0.8f, 0.5f);
        }).OnComplete(() => Time.timeScale = 0);
        base.OnShow();
    }

    public override void OnHide()
    {
        baseDialogAnimation.GetComponent<RectTransform>().DOScale(0, 0.5f).OnStart(() => {
            bgPanel.DOFade(0, 0.5f);
        }).OnComplete(() => base.OnHide());
    }

    void OnHomeButtonClick()
    {
        DialogManager.Instance.HideDialog(this, () =>
        {
            Time.timeScale = 1;
        });

        LoadSceneManager.Instance.OnSceneLoad("Buffer", (obj) =>
        {
            ViewManager.Instance.SwitchView(ViewIndex.HomeView);
        });
    }

    void OnContinueButtonClick()
    {
        DialogManager.Instance.HideDialog(this, () =>
        {
            Time.timeScale = 1;
        });
    }

    void OnClickMusicToggle(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt(Global.Data_Music, 0);
        }
        else
        {
            PlayerPrefs.SetInt(Global.Data_Music, 1);
        }
        SoundManager.Instance.PlayBackGroundMusic(isOn);
    }

    void OnClickSoundToggle(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt(Global.Data_Sound, 0);
        }
        else
        {
            PlayerPrefs.SetInt(Global.Data_Sound, 1);
        }
        SoundManager.Instance.isOnSound = isOn;
    }

    private void OnDisable()
    {
        homeButton.onClick.RemoveListener(OnHomeButtonClick);
        continueButton.onClick.RemoveListener(OnContinueButtonClick);
        musicToggle.onValueChanged.RemoveListener(OnClickMusicToggle);
        soundToggle.onValueChanged.RemoveListener(OnClickSoundToggle);
    }
}
