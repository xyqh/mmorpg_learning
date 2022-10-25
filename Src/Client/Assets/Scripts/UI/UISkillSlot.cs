using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Common.Data;

public class UISkillSlot : MonoBehaviour, IPointerClickHandler
{
    public Image skillIcon;
    public Image mask;
    public Text cd;
    
    private SkillDefine skillDefine = new SkillDefine();
    private float cdRemainTime = .0f;

    // Use this for initialization
    void Start () {
		
	}

    public void updateShow(SkillDefine skillDefine)
    {
        this.skillDefine = skillDefine;
        this.skillIcon.sprite = Resloader.Load<Sprite>(this.skillDefine.Icon);
    }
	
	// Update is called once per frame
	void Update () {
		if(this.cdRemainTime > .0f)
        {
            this.mask.enabled = true;
            this.cd.enabled = true;
            this.mask.fillAmount = this.cdRemainTime / this.skillDefine.CD;
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
        Debug.LogFormat("点击了技能：{0}，剩余冷却时间为：{1}", this.skillDefine.Name, this.cdRemainTime);
        if (cdRemainTime > .0f)
        {
            Debug.LogFormat("技能{0}cd中", this.skillDefine.Name);
        }
        else
        {
            Debug.LogFormat("技能{0}释放成功", this.skillDefine.Name);
            this.cdRemainTime = this.skillDefine.CD;
        }
    }
}
