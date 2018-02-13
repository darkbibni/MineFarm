using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using DG.Tweening;
using System;

public class UiManager : MonoBehaviour {
    
    [Header("Authentification")]
    public GameObject authentificationPanel;
    public InputField nickname;
    public InputField password;
    public Text alertMessage;
    
    // TODO separate scenes.
    [Header("Game")]
    public GameObject gamePanel;
    public Text goldText;
    public Text diamondText;
    public Text amethystText;

    public Image rock;

    public Image gaugeFilling;

    private Tween gaugeTween;
    private Tween alertTween;

    private Color fullOpacity;
    private Color noOpacity;

    public MineFarmGameManager gameMgr;

    public GameObject CurrentPanel
    {
        get
        {
            return CurrentPanel;
        }
        set
        {
            if(currentPanel != null)
            {
                currentPanel.SetActive(false);
            }

            currentPanel = value;
            currentPanel.SetActive(true);
        }
    }
    private GameObject currentPanel;

    private bool canMine = true;

	void Awake () {

        noOpacity = fullOpacity = alertMessage.color;
        noOpacity.a = 0f;
        fullOpacity.a = 1f;

        // Default panel.
        currentPanel = authentificationPanel;
    }

    public bool CheckAuthentificationForm()
    {
        return !(password.text.Equals("") || nickname.text.Equals(""));
    }
    
    public void UpdateInventory()
    {
        goldText.text = gameMgr.UserData.gold.ToString();
        diamondText.text = gameMgr.UserData.diamond.ToString();

        // TODO DEBUG TIME RECEIVED ! Then check remind time. Trigger animation.
        // Debug.Log(user.lastTimeMined);
        // TimeSpan diff = DateTime.Now - user.lastTimeMined;

        //Debug.Log(diff.TotalSeconds);

        //TriggerGauge((float) diff.TotalSeconds);
    }

    // DO a juicy animation !
    public void DisplayReward(int goldMined, int diamondMined)
    {
        UpdateInventory();

        // Restart gauge time.
        TriggerGauge(5f, true);
    }

    public void ShakeRock()
    {
        rock.transform.DOShakePosition(0.75f, 100, 50);
    }

    private void TriggerGauge(float remindTime, bool force = false)
    {
        if(force)
        {
            gaugeTween.Kill();
        }

        if(gaugeTween == null || !gaugeTween.IsPlaying())
        {
            gaugeFilling.fillAmount = 0f;
            gaugeTween = gaugeFilling.DOFillAmount(1f, remindTime).SetEase(Ease.Linear);
        }

        else
        {
            // Feedback 
        }
    }

    public void DisplayAlert(string message)
    {
        CancelInvoke("Disappear");

        if(alertTween != null && alertTween.IsPlaying())
        {
            alertTween.Kill();
        }

        alertMessage.text = message;
        alertMessage.color = fullOpacity;
        Invoke("Disappear", 1f);
    }

    private void Disappear()
    {
        alertTween = alertMessage.DOColor(noOpacity, 1f);
    }
}
