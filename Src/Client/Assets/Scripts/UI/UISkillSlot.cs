using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Common.Data;
using Battle;
using Common.Battle;
using SkillBridge.Message;
using Managers;

public class UISkillSlot : MonoBehaviour, IPointerClickHandler
{
    public Image skillIcon;
    public Image mask;
    public Text cd;
    
    private Skill skill = null;

    // Use this for initialization
    void Start () {
        this.mask.enabled = false;
        this.cd.enabled = false;
    }

    public void updateShow(Skill skill)
    {
        this.skill = skill;
        this.skillIcon.sprite = Resloader.Load<Sprite>(this.skill.Define.Icon);
    }
	
	// Update is called once per frame
	void Update () {
		if(this.skill.CD > 0)
        {
            if(!this.mask.enabled) this.mask.enabled = true;
            if (!this.cd.enabled) this.cd.enabled = true;
            this.mask.fillAmount = this.skill.CD / this.skill.Define.CD;
            this.cd.text = string.Format("{0:f1}", this.skill.CD);
        }
        else
        {
            if(this.mask.enabled) this.mask.enabled = false;
            if (this.cd.enabled) this.cd.enabled = false;
        }
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.LogFormat("点击了技能：{0}，剩余冷却时间为：{1}", this.skill.Define.Name, this.skill.CD);
        SkillResult result = this.skill.CanCast(BattleManager.Instance.CurrentTarget);
        switch (result)
        {
            case SkillResult.InvalidTarget:
                Debug.LogFormat("技能{0}目标无效", this.skill.Define.Name);
                break;
            case SkillResult.InvalidPosition:
                Debug.LogFormat("技能{0}位置无效", this.skill.Define.Name);
                break;
            case SkillResult.LackOfMp:
                Debug.LogFormat("技能{0}MP不足", this.skill.Define.Name);
                break;
            case SkillResult.CoolDown:
                Debug.LogFormat("技能{0}冷却中", this.skill.Define.Name);
                break;
            case SkillResult.Ok:
                Debug.LogFormat("技能{0}释放成功", this.skill.Define.Name);
                BattleManager.Instance.CastSkill(this.skill);
                break;
        }
    }
}
