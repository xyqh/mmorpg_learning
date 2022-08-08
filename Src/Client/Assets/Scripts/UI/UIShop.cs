using AillieoUtils;
using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour {

    public ScrollView scrollView;
    public Text shopName;
    public Button buy;
    public int selShopItemId = -1;

    private int shopId = -1;
    private ShopDefine shopDefine = null;
    private List<ShopItemDefine> itemDatas = new List<ShopItemDefine>();
    private Dictionary<int, ShopItemDefine> itemDatas_dic = new Dictionary<int, ShopItemDefine>();

	// Use this for initialization
	void Start () {
        scrollView.SetUpdateFunc((index, rectTransform) =>
        {
            int lowBit = index * 2 + 1, highBit = (index + 1) * 2;
            for (int i = lowBit; i <= highBit; ++i)
            {
                int suffix = (i - 1) % 2 + 1;
                GameObject item = rectTransform.Find("shopItem" + suffix.ToString()).gameObject;
                if (i > itemDatas.Count)
                {
                    item.SetActive(false);
                }
                else
                {
                    ShopItemDefine itemData = itemDatas[i - 1];
                    item.GetComponent<UIShopItem>().UpdateShow(itemData, this);
                    item.tag = itemData.ShopItemID.ToString();
                    item.SetActive(true);
                }
            }
        });

        scrollView.SetItemSizeFunc((index) =>
        {
            return new Vector2(472, 90);
        });

        scrollView.SetItemCountFunc(() =>
        {
            int count = (int)Math.Ceiling((double)itemDatas.Count / 2);
            return count;
        });
    }

    public void RefreshSelectItem(int shopItemID)
    {
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    void InitData(int shopId)
    {
        this.shopId = shopId;
        this.shopDefine = DataManager.Instance.IShops[this.shopId];
        this.itemDatas_dic = DataManager.Instance.IShopItems[this.shopId];

        this.shopName.text = this.shopDefine.Name;

        this.itemDatas.Clear();
        foreach(var kv in this.itemDatas_dic)
        {
            this.itemDatas.Add(kv.Value);
        }

        this.scrollView.UpdateData();
    }
}
