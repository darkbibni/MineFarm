using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineFarmGameManager : MonoBehaviour {
    
    public UiManager uiMgr;

    [Header("Mine parameters")]
    public float[] cooldowns;

    [Header("Vfx")]
    public Transform vfxParent;
    public ParticleSystem[] goldVfx;
    public ParticleSystem[] diamondVfx;
    public ParticleSystem[] amethystVfx;

    // TODO VFX !!!
    
    public UserData UserData
    {
        get { return userData; }
        set
        {
            userData = value;

            // Display rocks unlocked.
            SetupRocks();

            uiMgr.RefreshUI();
        }
    }
    private UserData userData;
    
    // TODO Unlock rock depending player data.
    private bool[] canMine;

    private void Awake()
    {
        int nbOfRocks = 7; // Get rock data.

        canMine = new bool[nbOfRocks];

        for (int i = 0; i < canMine.Length; i++)
        {
            canMine[i] = true;
        }
    }

    private void SetupRocks()
    {
        for (int i = 0; i < uiMgr.rocks.Length; i++)
        {
            if(i <= userData.rockBought)
            {
                uiMgr.rocks[i].gameObject.SetActive(true);
                uiMgr.hammers[i].gameObject.SetActive(true);
            }

            else
            {
                uiMgr.rocks[i].gameObject.SetActive(false);
                uiMgr.hammers[i].gameObject.SetActive(false);
            }
        }
    }

    public void UnlockRock()
    {
        userData.rockBought++;
        
        uiMgr.rocks[userData.rockBought].gameObject.SetActive(true);
        uiMgr.hammers[userData.rockBought].gameObject.SetActive(true);
    }

    // TODO Do it with a rock index.
    private IEnumerator CooldownMining(int rockIndex)
    {
        canMine[rockIndex] = false;

        yield return new WaitForSecondsRealtime(cooldowns[rockIndex]);

        canMine[rockIndex] = true;
    }

    /// <summary>
    /// Manager mine reward.
    /// </summary>
    /// <param name="mineId"></param>
    public void Mine(int rockIndex, RewardData rewardData)
    {
        // Prevent to mine the same rock.
        canMine[rockIndex] = false;

        // Run cooldown.
        StartCoroutine(CooldownMining(rockIndex));

        // Feedback mining.
        uiMgr.rocks[rockIndex].ShakeRock();

        // Run gauge.
        uiMgr.hammers[rockIndex].TriggerGauge(cooldowns[rockIndex], true);

        // Display the reward of the specified rock.
        MineEffect(rockIndex, rewardData.goldObtained, rewardData.diamondObtained, rewardData.amethystObtained);
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
    /// Trigger the mine effect. Reveal the reward and refresh inventory.
    /// </summary>
    /// <param name="goldMined"></param>
    /// <param name="diamondMined"></param>
    public void MineEffect(int rockId, int goldMined, int diamondMined, int amethystMined)
    {
        userData.gold += goldMined;
        userData.diamond += diamondMined;
        userData.amethyst += amethystMined;

        // TODO Feedback text on inventory (+amount)

        if(goldMined > 0)
        {
            goldVfx[rockId].Emit(goldMined);
        }

        if (diamondMined > 0)
        {
            diamondVfx[rockId].Emit(diamondMined);
        }

        if (amethystMined > 0)
        {
            amethystVfx[rockId].Emit(amethystMined);
        }

        uiMgr.RefreshUI();
    }
    
    public void DisplayGamePanel()
    {
        uiMgr.CurrentPanel = uiMgr.gamePanel;
    }
}
