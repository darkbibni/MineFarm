﻿using System.Collections;
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
            uiMgr.DisplayAlert("Some fields are missing !");
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
            uiMgr.DisplayAlert("Some fields are missing !");
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

    public void QuitEventButton()
    {
        Application.Quit();
    }

    public void MineButtonEvent()
    {
        if (api.SessionCorrect)
        {
            api.StartCoroutine(api.TryToMine(0));
        }

        else
        {
            uiMgr.CurrentPanel = uiMgr.authentificationPanel;
        }
    }

    public void ToggleLeaderboardEvent()
    {
        uiMgr.DisplayLeaderboard(true);

        // TODO Feedback loading...

        RefreshLeaderboard(ResourceType.GOLD);
    }

    public void LeaderboardGoldButtonEvent()
    {
        RefreshLeaderboard(ResourceType.GOLD);
    }

    public void LeaderboardDiamondButtonEvent()
    {
        RefreshLeaderboard(ResourceType.DIAMOND);
    }

    public void LeaderboardAmethystButtonEvent()
    {
        RefreshLeaderboard(ResourceType.AMEHTYST);
    }

    public void RefreshLeaderboard(ResourceType resource)
    {
        // TODO cooldown to prevent to refresh too many times !

        api.StartCoroutine(api.GetLeaderboard(resource));
    }

    public void BackLbButtonEvent()
    {
        uiMgr.DisplayLeaderboard(false);
    }
}
