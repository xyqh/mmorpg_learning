using Common.Data;
using Models;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEquipItem : MonoBehaviour, IPointerClickHandler
{
    public Image equipIcon;
    public Text equipName;

    //private int equipId = -1;
    private EquipDefine equipDefine;
    private float lastTime = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void updateShow(EquipDefine equipDefine)
    {
        this.equipDefine = equipDefine;

        equipIcon.sprite = Resources.Load<Sprite>(string.Format("UI/Items/equip{0}.png", equipDefine.ID));
        equipName.text = equipDefine.Name;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(Time.realtimeSinceStartup - lastTime < 0.3f)
        {
            Debug.Log("Double Click!!!");
            ItemService.Instance.SendEquipItem(new Item(equipDefine.ID, 1), true);
        }

        lastTime = Time.realtimeSinceStartup;
        Debug.Log("Single Click!!!");
    }
}
