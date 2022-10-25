using AillieoUtils;
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
    private List<SkillDefine> defines = new List<SkillDefine>();
    private Dictionary<int, SkillDefine> skillMap = new Dictionary<int, SkillDefine>();
    private int characterClass;

    // Use this for initialization
    void Start() {
        EventManager.Instance.addEventListener("onClickSkillItemInSkillView", this.updateSkillDesc);
        this.characterClass = (int)User.Instance.CurrentCharacterInfo.Class;
        DataManager.Instance.ISkills.TryGetValue(this.characterClass, out skillMap);
        if (skillMap != null)
        {
            foreach (var kv in skillMap)
            {
                defines.Add(kv.Value);
            }
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
            transform.GetComponent<UISkillItem>().updateView(this.defines[index].ID);
        });

        scrollView.UpdateData();
    }

    private void updateSkillDesc(object[] param)
    {
        int skillId = (int)param[0];
        this.skillDesc.text = skillMap[skillId].Description;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
