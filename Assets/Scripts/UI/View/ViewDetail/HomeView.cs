using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HomeView : BaseView
{
    public event Action<bool> OnSoundChangeHanle;
    public event Action<bool> OnMusicChangeHanle;

    [SerializeField] private Button playButton;

    [SerializeField] private GameObject rotatorEffect;
    [SerializeField] private RectTransform iconGame;
    [SerializeField] private RectTransform soundPanelContainer;
    [SerializeField] private float speedRotate;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle soundToggle;

    private Vector3 angle = new Vector3(0f, 0f, 45f);

    private void Update()
    {
        rotatorEffect.transform.localEulerAngles += angle * Time.deltaTime * speedRotate;
      
    }
    
    private void OnEnable()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        musicToggle.onValueChanged.AddListener(OnClickMusicToggle);
        soundToggle.onValueChanged.AddListener(OnClickSoundToggle);
    }

    //public override void Setup(ViewParam param, Action callBack = null)
    //{
    //    playButton.GetComponent<RectTransform>().DOAnchorPosX(-1066, 0f);
    //    soundPanelContainer.DOAnchorPosX(957, 0f);
    //    iconGame.DOAnchorPosY(1222f, 1f).OnComplete(() =>
    //    {
    //        rotatorEffect.transform.DOScale(Vector3.zero, 0f);
    //    });
    //    base.Setup(param, callBack);
    //}

    public override void OnShow(Action callBack)
    {
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

        playButton.GetComponent<RectTransform>().DOAnchorPosX(0, 1f);
        soundPanelContainer.DOAnchorPosX(0, 1f);
        iconGame.DOAnchorPosY(-146f, 1f).OnComplete(() =>
        {
            rotatorEffect.transform.DOScale(Vector3.one, 1.5f);
        });
        base.OnShow(callBack);
    }

    //public override void OnHide(Action callBack)
    //{
    //    //baseAnimation.GetComponent<RectTransform>().DOAnchorPosX(-1111f, 0.5f).OnComplete(() =>
    //    //{
    //    //    base.OnHide(callBack);
    //    //});
    //}

    void OnPlayButtonClick()
    {
        ViewManager.Instance.SwitchView(ViewIndex.LevelView);
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
        playButton.onClick.RemoveListener(OnPlayButtonClick);
        musicToggle.onValueChanged.RemoveListener(OnClickMusicToggle);
        soundToggle.onValueChanged.RemoveListener(OnClickSoundToggle);
    }
}
