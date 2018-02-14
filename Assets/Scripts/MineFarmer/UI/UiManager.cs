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
    
    public Hammer[] hammers;
    public Rock[] rocks;

    [Header("Leaderboard")]
    public GameObject leaderboardPopup;
    public Text leaderboardTitle;
    public GameObject entriesParent;
    public Text[] entries;

    [Header("Shop")]
    public GameObject shopPopup;

    #endregion

    private Tween alertTween;

    // Opacity of the alert message.
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
            if(currentPanel)
            {
                currentPanel.SetActive(true);
            }
        }
    }
    private GameObject currentPanel;

    public GameObject CurrentPopup
    {
        get
        {
            return currentPopup;
        }
        set
        {
            if (currentPopup != null)
            {
                currentPopup.SetActive(false);
            }

            currentPopup = value;

            if (currentPopup)
            {
                currentPopup.SetActive(true);
            }
        }
    }
    private GameObject currentPopup;

    void Awake () {

        noOpacity = fullOpacity = alertMessage.color;
        noOpacity.a = 0f;
        fullOpacity.a = 1f;

        // Default panel.
        currentPanel = authentificationPanel;

        // Restore nickname.
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

    public void RefreshUI()
    {
        goldText.text = gameMgr.UserData.gold.ToString();
        diamondText.text = gameMgr.UserData.diamond.ToString();
        amethystText.text = gameMgr.UserData.amethyst.ToString();

        // TODO DEBUG TIME RECEIVED ! Then check remind time. Trigger animation.
        // Debug.Log(user.lastTimeMined);
        // TimeSpan diff = DateTime.Now - user.lastTimeMined;

        //Debug.Log(diff.TotalSeconds);

        //TriggerGauge((float) diff.TotalSeconds);
    }

    #endregion

    #region Leaderboard components

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

            case ResourceType.AMEHTYST: amount = user.amethyst; break;

            default: amount = 0; break;
        }

        return amount;
    }

    #endregion
}
