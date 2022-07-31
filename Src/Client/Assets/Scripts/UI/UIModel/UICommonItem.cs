using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICommonItem : MonoBehaviour {

    public Image icon;
    public Text num;
    public ItemDefine itemDefine;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateShow(int itemId, int num = 0, bool isShowNum = false)
    {
        itemDefine = DataManager.Instance.IItems[itemId];

        UpdateItemIcon();
        UpdateItemNum(num);
    }

    public void UpdateItemIcon()
    {
        icon.sprite = Resources.Load<Sprite>(itemDefine.Icon);
    }

    public void UpdateItemNum(int num)
    {
        this.num.text = num.ToString();
    }
}
