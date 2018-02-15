using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour {

    public UiManager ui;

    public Image buttonImage;
    public Image rockImage;
    public Button button;
    public Text price;

    private Sprite originalRockSprite;

    #region Private attributes

    void Reset()
    {
        buttonImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        rockImage = buttonImage.transform.GetChild(0).GetComponent<Image>();
        button = buttonImage.GetComponent<Button>();
        price = transform.GetChild(1).GetComponent<Text>();
    }

    #endregion

    public void StoreOriginalSprite()
    {
        // Keep the sprite on memory.
        originalRockSprite = rockImage.sprite;
    }

    /// <summary>
    /// Call this when the object is locked. Unbuyable.
    /// </summary>
    public void FeedbackLocked()
    {
        // Keep the sprite on memory.
        originalRockSprite = rockImage.sprite;

        button.interactable = false;
        rockImage.sprite = ui.rockLocked;
        buttonImage.color = ui.itemLockedColor;
    }

    /// <summary>
    /// Call this when the object can be buy by the player.
    /// </summary>
    public void FeedbackBuyable()
    {
        button.interactable = true;
        rockImage.sprite = originalRockSprite;
        buttonImage.color = ui.itemBuyable;
    }

    /// <summary>
    /// Call this when the object is bought.
    /// </summary>
    public void FeedbackBought()
    {
        button.interactable = false;
        buttonImage.color = ui.itemBoughtColor;
    }
}
