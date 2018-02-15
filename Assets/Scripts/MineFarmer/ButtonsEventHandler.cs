using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsEventHandler : MonoBehaviour {

    public UiManager uiMgr;
    public MineFarmAPIClient api;

    /// <summary>
    /// Check if fields are correct. Then try to register.
    /// </summary>
    public void RegisterButtonEvent()
    {
        if (uiMgr.CheckAuthentificationForm())
        {
            api.StartCoroutine(api.TryToRegister(uiMgr.nickname.text, uiMgr.password.text));
        }

        else
        {
            uiMgr.MainMenuFeedback("Some fields are missing !");
        }
    }

    /// <summary>
    /// Check if fields are correct. Then try to connect.
    /// </summary>
    public void ConnectButtonEvent()
    {
        if (uiMgr.CheckAuthentificationForm())
        {
            // Save nickname if player has check the 
            if (uiMgr.keepNickname.isOn)
            {
                PlayerPrefs.SetString("Nickname", uiMgr.nickname.text);
            }

            var t = api.TryToConnect(uiMgr.nickname.text, uiMgr.password.text);
        }

        else
        {
            uiMgr.MainMenuFeedback("Some fields are missing !");
        }
    }

    /// <summary>
    /// Clear session informations. Back to authentification panel.
    /// </summary>
    public void DisconnectEventButton()
    {
        api.ResetSession();

        uiMgr.CurrentPanel = uiMgr.authentificationPanel;
    }

    /// <summary>
    /// Quit the application.
    /// </summary>
    public void QuitEventButton()
    {
        Application.Quit();
    }

    /// <summary>
    /// Try to mine the specified rock.
    /// </summary>
    /// <param name="rockId"></param>
    public void MineButtonEvent(int rockId)
    {
        if (api.SessionCorrect)
        {
            api.StartCoroutine(api.TryToMine(rockId));
        }

        else
        {
            uiMgr.CurrentPanel = uiMgr.authentificationPanel;
        }
    }

    /// <summary>
    /// Display the leaderboard popup.
    /// </summary>
    public void ToggleLeaderboardEvent()
    {
        uiMgr.CurrentPopup = uiMgr.leaderboardPopup;

        // TODO Feedback loading...

        RefreshLeaderboard(ResourceType.GOLD);
    }

    /// <summary>
    /// Refresh the leaderboard for golds.
    /// </summary>
    public void LeaderboardGoldButtonEvent()
    {
        RefreshLeaderboard(ResourceType.GOLD);
    }

    /// <summary>
    /// Refresh the leaderboard for diamonds.
    /// </summary>
    public void LeaderboardDiamondButtonEvent()
    {
        RefreshLeaderboard(ResourceType.DIAMOND);
    }

    /// <summary>
    /// Refresh the leaderboard for amethysts.
    /// </summary>
    public void LeaderboardAmethystButtonEvent()
    {
        RefreshLeaderboard(ResourceType.AMEHTYST);
    }

    /// <summary>
    /// Display the shop.
    /// </summary>
    public void ToggleShopEvent()
    {
        uiMgr.CurrentPopup = uiMgr.shopPopup;
    }

    /// <summary>
    /// Close the popup.
    /// </summary>
    public void BackButtonEvent()
    {
        uiMgr.CurrentPopup = null;
    }

    public void BuyARock()
    {
        api.StartCoroutine(api.BuyRock());
    }

    private void RefreshLeaderboard(ResourceType resource)
    {
        // TODO cooldown to prevent to refresh too many times !

        api.StartCoroutine(api.GetLeaderboard(resource));
    }
}
