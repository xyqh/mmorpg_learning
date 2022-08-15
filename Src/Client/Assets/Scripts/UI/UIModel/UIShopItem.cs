using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour
{

    public Image imageBtm;
    public UICommonItem goItem;
    public Text textItemName;
    public Text textItemPrice;

    private UIShop owner;
    private ShopItemDefine shopItemDefine;
    private int itemId = -1;
    private bool selected = false;

	// Use this for initialization
	void Start () {
        EventManager.Instance.addEventListener("ShopOnSelectItem", this.onSelectItem);
	}

    // Update is called once per frame
    void Update () {
		
	}

    public void UpdateShow(ShopItemDefine shopItemDefine, UIShop owner)
    {
        this.owner = owner;
        this.shopItemDefine = shopItemDefine;

        this.itemId = this.shopItemDefine.ItemID;
        goItem.UpdateShow(this.itemId, this.shopItemDefine.Count);

        // 更新道具名称价格
        textItemName.text = DataManager.Instance.IItems[this.itemId].Name;
        textItemPrice.text = shopItemDefine.Price.ToString();
    }

    public void refreshIsSelected(bool isSel)
    {
        EventManager.Instance.dispatchCustomEvent("ShopOnSelectItem", this.shopItemDefine.ShopItemID);
    }

    private void onSelectItem(object[] param)
    {
        if (this.shopItemDefine == null) return;

        int selShopItemId = (int)param[0];

        bool isSel = this.shopItemDefine.ShopItemID == selShopItemId;
        if (this.selected == isSel) return;

        this.selected = isSel;
        if (this.selected)
        {
            owner.refreshSelectedShopItemId(this.shopItemDefine.ShopItemID);
            this.imageBtm.overrideSprite = Resources.Load<Sprite>("UI/common/common_bg_03");
        }
        else
        {
            this.imageBtm.overrideSprite = null;
        }
    }
}
