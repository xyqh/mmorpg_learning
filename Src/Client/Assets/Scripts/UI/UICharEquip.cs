using AillieoUtils;
using Common.Data;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharEquip : MonoBehaviour {

    public Dictionary<EquipSlot, Transform> slots = new Dictionary<EquipSlot, Transform>();

    List<EquipDefine> equipDefines;
    ScrollView scrollView;

	// Use this for initialization
	void Start () {
        // 初始化所有装备槽位的Transform
        for(EquipSlot slotType = 0; slotType < EquipSlot.SlotMax; ++slotType)
        {
            Transform transform = this.gameObject.transform.Find(string.Format("Slot{0}", slotType));
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
}
