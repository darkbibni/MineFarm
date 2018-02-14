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
    public ShopItem[] rocksToBuy;
    public Sprite rockLocked;
    public Color itemBoughtColor;
    public Color itemLockColor;

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
        // Refresh inventory.
        goldText.text = gameMgr.UserData.gold.ToString();
        diamondText.text = gameMgr.UserData.diamond.ToString();
        amethystText.text = gameMgr.UserData.amethyst.ToString();

        // Refresh shop.
        for (int i = 0; i < rocksToBuy.Length; i++)
        {
            if(i <= gameMgr.UserData.rockBought)
            {
                rocksToBuy[i].FeedbackBought();
            }

            if(i > gameMgr.UserData.rockBought+1)
            {
                rocksToBuy[i].FeedbackLocked();
            }
        }

        // TODO DEBUG TIME RECEIVED ! Then check remind time. Trigger animation.
        // Debug.Log(user.lastTimeMined);
        // TimeSpan diff = DateTime.Now - user.lastTimeMined;

        //Debug.Log(diff.TotalSeconds);

        //TriggerGauge((float) diff.TotalSeconds);
    }

    /// <summary>
    /// Lock the specified item of the specified category.
    /// </summary>
    /// <param name="catergoryId"></param>
    /// <param name="itemId"></param>
    public void BuyShopitem(int catergoryId, int itemId)
    {
        switch(catergoryId)
        {
            case 0: rocksToBuy[itemId].FeedbackBought(); break;
            case 1: break;
            case 2: break;
            default: break;
        }
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


    [ContextMenu("Fake shop")]
    public void FakeShop()
    {
        UserData data = new UserData()
        {
            rockBought = 3, // Has buy 3 rocks.
        };

        // Refresh shop.
        for (int i = 0; i < rocksToBuy.Length; i++)
        {
            if (i <= data.rockBought-1)
            {
                rocksToBuy[i].FeedbackBought();
            }

            if (i > data.rockBought)
            {
                rocksToBuy[i].FeedbackLocked();
            }
        }
    }
}
