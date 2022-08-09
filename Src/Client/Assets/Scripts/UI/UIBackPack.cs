using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AillieoUtils;
using System;
using Random = UnityEngine.Random;
using Models;
using Managers;

public class UIBackPack : UIWindow
{
    
    public ScrollView scrollView;
    List<BagItem> itemDatas = new List<BagItem>();

    // Use this for initialization
    void Start () {

        //InitData();

        scrollView.SetUpdateFunc((index, rectTransform) =>
        {
            int lowBit = index * 4 + 1, highBit = (index + 1) * 4;
            for (int i = lowBit; i <= highBit; ++i)
            {
                int suffix = (i - 1) % 4 + 1;
                GameObject item = rectTransform.Find("item" + suffix.ToString()).gameObject;
                if (i > itemDatas.Count)
                {
                    item.SetActive(false);
                }
                else
                {
                    BagItem itemData = itemDatas[i - 1];
                    item.GetComponent<UICommonItem>().UpdateShow(itemData.itemId, itemData.count, true);
                    item.SetActive(true);
                }
            }
        });

        scrollView.SetItemSizeFunc((index) =>
        {
            return new Vector2(372, 90);
        });

        scrollView.SetItemCountFunc(() =>
        {
            int count = (int)Math.Ceiling((double)itemDatas.Count / 4);
            return count;
        });

        ReloadBackPack();
    }

    private void OnEnable()
    {
        ReloadBackPack();
    }

    public void ReloadBackPack()
    {
        this.itemDatas = BagManager.Instance.bagItems;
        scrollView.UpdateData();
    }

    public void UpdateMoney()
    {

    }

    // Update is called once per frame
    void Update () {
		
	}

    void InitData()
    {
        for(int i = 0; i < 5; ++i)
        {
            BagItem data = new BagItem()
            {
                itemId = (ushort)(i % 10 + 1),
                count = (ushort)Random.Range(1, 100)
            };

            itemDatas.Add(data);
        }
    }
}
