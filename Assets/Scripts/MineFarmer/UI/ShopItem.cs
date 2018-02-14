using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour {

    public UiManager ui;

    public Image buttonImage;
    public Image rockImage;
    public Button button;

    private Sprite originalRockSprite;

    #region Private attributes

    void Reset()
    {
        buttonImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        rockImage = buttonImage.transform.GetChild(0).GetComponent<Image>();
        button = buttonImage.GetComponent<Button>();
    }

    void Awake()
    {
        originalRockSprite = rockImage.sprite;
    }

    #endregion

    public void FeedbackLocked()
    {
        button.interactable = false;
        rockImage.sprite = ui.rockLocked;
        buttonImage.color = ui.itemLockColor;
    }

    public void FeedbackBought()
    {
        button.interactable = false;
        rockImage.sprite = originalRockSprite;
        buttonImage.color = ui.itemBoughtColor;
    }
}
