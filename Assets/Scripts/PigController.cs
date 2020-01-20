using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PigController : SingletonMono<PigController>
{
    public RectTransform pig;
    public RectTransform finishPoint;
    public Image finishFlag;
    public Sprite[] flagAnim;
    public float distance;
    Vector2 oldPosMax;

    private void Start()
    {
        distance = pig.rect.position.x;
        StopCoroutine("FlagAnimation");
        StartCoroutine("FlagAnimation");
        oldPosMax = pig.anchoredPosition;
    }

    public void UpdatePigPosition(float currentScore, float maxScore)
    {
        float percent = Mathf.InverseLerp(0f, maxScore, currentScore);
        float rectXPig = percent * finishPoint.anchoredPosition.x;
        pig.DOAnchorPosX(rectXPig, 0.5f);

    }

    IEnumerator FlagAnimation()
    {
        while (true)
        {
            for (int i = 0; i < flagAnim.Length; i++)
            {
                finishFlag.sprite = flagAnim[i];
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
