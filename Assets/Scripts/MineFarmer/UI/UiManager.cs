using UnityEngine;

using UnityEngine.UI;

using DG.Tweening;

/// <summary>
/// Manage all ui components of the game.
/// </summary>
public class UiManager : MonoBehaviour {

    #region Inspector attributes

    public MineFarmGameManager gameMgr;

    [Header("Authentification")]
    public GameObject authentificationPanel;
    public InputField nickname;
    public InputField password;
    public Toggle keepNickname;
    public Text alertMessage;
    
    [Header("Game")]
    public GameObject gamePanel;
    public Text goldText;
    public Text diamondText;
    public Text amethystText;

    public Image rock;

    public Image gaugeFilling;

    [Header("Leaderboard")]
    public GameObject leaderboardPopup;
    public Text leaderboardTitle;
    public GameObject entriesParent;
    public Text[] entries;

    #endregion

    private Tween gaugeTween;
    private Tween alertTween;

    private Color fullOpacity;
    private Color noOpacity;

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

	void Awake () {

        noOpacity = fullOpacity = alertMessage.color;
        noOpacity.a = 0f;
        fullOpacity.a = 1f;

        // Default panel.
        currentPanel = authentificationPanel;

        string nicknameSaved = PlayerPrefs.GetString("Nickname");
        nickname.text = nicknameSaved;
    }

    #region Authentification components

    public bool CheckAuthentificationForm()
    {
        return !(password.text.Equals("") || nickname.text.Equals(""));
    }


    public void DisplayAlert(string message)
    {
        CancelInvoke("Disappear");

        if (alertTween != null && alertTween.IsPlaying())
        {
            alertTween.Kill();
        }

        alertMessage.text = message;
        alertMessage.color = fullOpacity;
        Invoke("Disappear", 2f);
    }

    private void Disappear()
    {
        alertTween = alertMessage.DOColor(noOpacity, 1f);
    }

    #endregion

    #region Game components

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

    public void DisplayReward(int goldMined, int diamondMined)
    {
        UpdateInventory();

        // Restart gauge time.
        TriggerGauge(gameMgr.mineCooldown, true);
    }

    public void ShakeRock()
    {
        rock.transform.DOShakePosition(0.75f, 100, 50).OnComplete(ResetRockPosition);
    }

    private void ResetRockPosition()
    {
        rock.transform.localPosition = Vector3.zero;
    }

    public void FeedbackCantMine()
    {
        rock.DOColor(Color.red, 0.25f).OnComplete(ResetRockColor);
    }

    private void ResetRockColor()
    {
        rock.color = Color.white;
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
    }

    #endregion

    #region Leaderboard components

    public void DisplayLeaderboard(bool displayed)
    {
        leaderboardPopup.SetActive(displayed);
    }

    // TODO calculate place depending exaequo.

    public void UpdateLeaderboard(ResourceType resource, UserData[] top)
    {
        string resourceName = ResourceUtility.GetName(resource);

        leaderboardTitle.text = "TOP 10 - " + resourceName;

        for (int i = 0; i < entries.Length; i++)
        {
            if(i < top.Length)
            {
                entries[i].gameObject.SetActive(true);
                entries[i].text = "[" + (i + 1) + "] " + top[i].name + " : " + GetResource(resource, top[i]) + " " + resourceName + "s";
            }

            else
            {
                entries[i].gameObject.SetActive(false);
            }
        }
    }

    private int GetResource(ResourceType resource, UserData user)
    {
        int amount = 0;

        switch (resource)
        {
            case ResourceType.GOLD: amount = user.gold; break;

            case ResourceType.DIAMOND: amount = user.diamond; break;

            case ResourceType.AMEHTYST: amount = 0; break;// TODO change that !

            default: amount = 0; break;
        }

        return amount;
    }

    #endregion
}
