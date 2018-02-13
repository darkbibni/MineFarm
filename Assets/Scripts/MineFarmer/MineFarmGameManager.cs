using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineFarmGameManager : MonoBehaviour {
    
    public UiManager uiMgr;

    [Header("Sfx")]
    public ParticleSystem goldVfx;
    public ParticleSystem diamondVfx;
    
    public UserData UserData
    {
        get { return userData; }
        set
        {
            userData = value;
            uiMgr.UpdateInventory();
        }
    }
    private UserData userData;
    
    // TODO add rock unlocked.
    private bool[] canMine;

    private void Awake()
    {
        int nbOfRocks = 1;
        canMine = new bool[nbOfRocks];

        for (int i = 0; i < canMine.Length; i++)
        {
            canMine[i] = true;
        }
    }

    // TODO Do it with a rock index.
    private IEnumerator CooldownMining(int rockIndex)
    {
        canMine[rockIndex] = false;

        yield return new WaitForSecondsRealtime(5f);

        canMine[rockIndex] = true;
    }


    /// <summary>
    /// Manager mine reward.
    /// </summary>
    /// <param name="mineId"></param>
    public void Mine(int rockIndex, int mineId)
    {
        // Prevent to mine the same rock.
        canMine[rockIndex] = false;

        // Run cooldown.
        StartCoroutine(CooldownMining(0));

        // Feedback mining.
        uiMgr.ShakeRock();

        // Get the reward.
        switch (mineId)
        {
            case 0: UpdateUser(1, 0); break;
            case 1: UpdateUser(0, 1); break;
            case 2: UpdateUser(5, 1); break;
            case 3: UpdateUser(50, 10); break;
        }
    }
    
    /// <summary>
    /// Return if the rock specified can be mined.
    /// </summary>
    /// <param name="rockIndex"></param>
    /// <returns></returns>
    public bool CanMineRock(int rockIndex)
    {
        return canMine[rockIndex];
    }

    /// <summary>
    /// Update the user data with a juicy animation.
    /// </summary>
    /// <param name="goldMined"></param>
    /// <param name="diamondMined"></param>
    public void UpdateUser(int goldMined, int diamondMined)
    {
        userData.gold += goldMined;
        userData.diamond += diamondMined;

        goldVfx.Emit(goldMined);
        diamondVfx.Emit(diamondMined);

        uiMgr.DisplayReward(goldMined, diamondMined);
    }
    
    public void DisplayGamePanel()
    {
        uiMgr.CurrentPanel = uiMgr.gamePanel;
    }

    public void FeedbackRegistered()
    {
        uiMgr.DisplayAlert("You are now registered !");
    }
}
