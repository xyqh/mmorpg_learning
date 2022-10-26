using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Common.Data;
using Battle;
using Common.Battle;

public class UISkillSlot : MonoBehaviour, IPointerClickHandler
{
    public Image skillIcon;
    public Image mask;
    public Text cd;
    
    private Skill skill = null;
    private float cdRemainTime = .0f;

    // Use this for initialization
    void Start () {
		
	}

    public void updateShow(Skill skill)
    {
        this.skill = skill;
        this.skillIcon.sprite = Resloader.Load<Sprite>(this.skill.Define.Icon);
    }
	
	// Update is called once per frame
	void Update () {
		if(this.cdRemainTime > .0f)
        {
            this.mask.enabled = true;
            this.cd.enabled = true;
            this.mask.fillAmount = this.cdRemainTime / this.skill.Define.CD;
            this.cd.text = string.Format("{0:f1}", this.cdRemainTime);
            this.cdRemainTime -= Time.deltaTime;
        }
        else
        {
            this.mask.enabled = false;
            this.cd.enabled = false;
        }
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.LogFormat("点击了技能：{0}，剩余冷却时间为：{1}", this.skill.Define.Name, this.cdRemainTime);
        SkillResult result = this.skill.CanCast();
        switch (result)
        {
            case SkillResult.InvalidTarget:
                Debug.LogFormat("技能{0}目标无效", this.skill.Define.Name);
                break;
            case SkillResult.InvalidPosition:
                Debug.LogFormat("技能{0}位置无效", this.skill.Define.Name);
                break;
            case SkillResult.LackOfMP:
                Debug.LogFormat("技能{0}MP不足", this.skill.Define.Name);
                break;
            case SkillResult.CoolDown:
                Debug.LogFormat("技能{0}冷却中", this.skill.Define.Name);
                break;
            case SkillResult.OK:
                Debug.LogFormat("技能{0}释放成功", this.skill.Define.Name);
                this.cdRemainTime = this.skill.Define.CD;
                this.skill.Cast();
                break;

        }
    }
}
