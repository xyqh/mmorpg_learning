using AillieoUtils;
using Common.Data;
using Managers;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharEquip : UIWindow
{

    public Dictionary<EquipSlot, Transform> slots = new Dictionary<EquipSlot, Transform>();

    List<EquipDefine> equipDefines = new List<EquipDefine>();
    public ScrollView scrollView;

	// Use this for initialization
	void Start () {
        // 监听装备更新的事件
        EventManager.Instance.addEventListener("EquipItemUpdate", updateEquipped);

        // 初始化所有装备槽位的Transform
        for (EquipSlot slotType = 0; slotType < EquipSlot.SlotMax; ++slotType)
        {
            string path = string.Format("ImageBg/PanelEquip/Slot{0}", (int)slotType);
            Transform transform = this.gameObject.transform.Find(path);
            if(transform != null)
            {
                slots[slotType] = transform;
            }
        }

        scrollView.SetUpdateFunc((index, rectTransform) =>
        {
            EquipDefine equipDefine = equipDefines[index];
            rectTransform.GetComponent<UIEquipItem>().updateShow(equipDefine);
        });

        scrollView.SetItemCountFunc(() =>
        {
            return equipDefines.Count;
        });

        scrollView.SetItemSizeFunc((index) =>
        {
            return new Vector2(377, 100);
        });

        this.ReloadScrollView();
	}

    void ReloadScrollView(List<EquipDefine> equipDefines = null)
    {
        if(equipDefines != null)
        {
            this.equipDefines = equipDefines;
        }

        scrollView.UpdateData();
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void equip(EquipSlot equipSlot, int id)
    {
        
    }

    public void disEquip(EquipSlot equipSlot)
    {

    }

    public void RefreshEquips()
    {
        this.ReloadScrollView(this.getEquips());
    }

    List<EquipDefine> getEquips()
    {
        List<BagItem> bagItems = BagManager.Instance.bagItems;
        List<EquipDefine> defines = new List<EquipDefine>();
        for(int i = 0; i < bagItems.Count; ++i)
        {
            if(bagItems[i].itemId > 1000 && bagItems[i].itemId < 5000)
            {
                defines.Add(DataManager.Instance.IEquips[bagItems[i].itemId]);
            }
        }

        return defines;
    }

    public void updateView()
    {
        this.RefreshEquips();
        this.updateEquipped();
    }

    private void updateEquipped(params object[] param)
    {
        for (EquipSlot slotType = 0; slotType < EquipSlot.SlotMax; ++slotType)
        {
            Item slotItem = EquipManager.Instance.GetEquip(slotType);
            if(slotItem != null)
            {
                ItemDefine itemDefine = DataManager.Instance.IItems[slotItem.id];
                slots[slotType].GetComponent<Image>().overrideSprite = Resources.Load<Sprite>(itemDefine.Icon);
            }
        }
    }

    void InitAttributes()
    {
        var charattr = User.Instance.CurrentCharacter.Attributes;
        return;
    }
}
