using AillieoUtils;
using Battle;
using Common.Data;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillView : UIWindow {

    public ScrollView scrollView;
    public Text skillDesc;
    private List<Skill> defines = new List<Skill>();
    private Dictionary<int, Skill> skillMap = new Dictionary<int, Skill>();
    private int characterClass;

    // Use this for initialization
    void Start() {
        EventManager.Instance.addEventListener("onClickSkillItemInSkillView", this.updateSkillDesc);
        var skills = User.Instance.CurrentCharacter.SkillMgr.Skills;
        for(int i = 0; i < skills.Count; ++i)
        {
            defines.Add(skills[i]);
            skillMap[skills[i].Define.ID] = skills[i];
        }

        scrollView.SetItemCountFunc(() =>
        {
            return this.defines.Count;
        });

        scrollView.SetItemSizeFunc((int index) =>
        {
            return new Vector2(208.26f, 80);
        });

        scrollView.SetUpdateFunc((int index, RectTransform transform) =>
        {
            transform.GetComponent<UISkillItem>().updateView(this.defines[index].Define.ID);
        });

        scrollView.UpdateData();
    }

    private void updateSkillDesc(object[] param)
    {
        int skillId = (int)param[0];
        this.skillDesc.text = skillMap[skillId].Define.Description;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
