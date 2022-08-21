using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEquipItem : MonoBehaviour, ISelectHandler
{
    public Image equipIcon;
    public Text equipName;

    private int equipId = -1;
    private EquipDefine equipDefine;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void updateShow()
    {
        equipIcon.sprite = Resources.Load<Sprite>(string.Format("UI/Items/equip{0}.png", equipDefine.ID));
        //equipName.text = equipDefine.
    }

    public void OnSelect(BaseEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
