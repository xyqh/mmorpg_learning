using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour, ISelectHandler
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

    public void OnSelect(BaseEventData eventData)
    {
        selected = !selected;
        this.owner.RefreshSelectItem(this.shopItemDefine.ShopItemID);
    }
}
