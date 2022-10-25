using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Common.Data;
using UnityEngine.EventSystems;
using System;

public class UISkillItem : MonoBehaviour, ISelectHandler
{

    public Image skillBg;
    public Image skillIcon;
    public Text skillName;
    public Text skillLevel;
    public Sprite normalBg;
    public Sprite selectedBg;

    private SkillDefine define = null;

	// Use this for initialization
	void Start () {
        EventManager.Instance.addEventListener("onClickSkillItemInSkillView", this.onClickSkill);
	}

    private void onClickSkill(object[] param)
    {
        if (this.define == null) return;
        int skillId = (int)param[0];
        this.skillBg.overrideSprite = skillId == this.define.ID ? selectedBg : normalBg;
    }

    public void updateView(int skillId)
    {
        DataManager.Instance.ISkillMap.TryGetValue(skillId, out this.define);
        if(this.define != null)
        {
            this.skillIcon.sprite = Resloader.Load<Sprite>(define.Icon);
            this.skillName.text = define.Name;
            this.skillLevel.text = define.UnlockLevel.ToString();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSelect(BaseEventData eventData)
    {
        if (this.define == null) return;
        EventManager.Instance.dispatchCustomEvent("onClickSkillItemInSkillView", this.define.ID);
    }
}
