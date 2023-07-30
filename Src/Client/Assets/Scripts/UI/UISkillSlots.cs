using Battle;
using Common.Data;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillSlots : MonoBehaviour {
    private List<UISkillSlot> slots = new List<UISkillSlot>();
    private List<Skill> defines = new List<Skill>();
    private Dictionary<int, Skill> skillMap = new Dictionary<int, Skill>();
    private int characterClass;

    // Use this for initialization
    void Start () {
        this.UpdateSkillShow();
        EventManager.Instance.addEventListener("updateSkillShow", this.UpdateSkillShow);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void UpdateSkillShow(params object[] param)
    {
        this.defines.Clear();
        var skills = User.Instance.CurrentCharacter.SkillMgr.Skills;
        for (int i = 0; i < skills.Count; ++i)
        {
            defines.Add(skills[i]);
            skillMap[skills[i].Define.ID] = skills[i];
        }

        for (int i = 1; i <= 4; ++i)
        {
            UISkillSlot slot = this.transform.Find(string.Format("UISkillSlot{0}", i)).GetComponent<UISkillSlot>();
            slot.updateShow(defines[i - 1]);
            slots.Add(slot);
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.removeEventListener("updateSkillShow", this.UpdateSkillShow);
    }
}
