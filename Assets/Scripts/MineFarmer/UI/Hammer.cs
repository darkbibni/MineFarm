using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class Hammer : MonoBehaviour {

    public Image gaugeFilling;
    private Tween gaugeTween;

    private void Reset()
    {
        gaugeFilling = transform.GetChild(1).GetComponent<Image>();
    }

    public void TriggerGauge(float remindTime, bool force = false)
    {
        if (force)
        {
            gaugeTween.Kill();
        }

        if (gaugeTween == null || !gaugeTween.IsPlaying())
        {
            gaugeFilling.fillAmount = 0f;
            gaugeTween = gaugeFilling.DOFillAmount(1f, remindTime).SetEase(Ease.Linear);
        }
    }
}
