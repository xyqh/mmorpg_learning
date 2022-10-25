using Common.Data;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillSlots : MonoBehaviour {
    private List<UISkillSlot> slots = new List<UISkillSlot>();
    private List<SkillDefine> defines = new List<SkillDefine>();
    private Dictionary<int, SkillDefine> skillMap = new Dictionary<int, SkillDefine>();
    private int characterClass;

    // Use this for initialization
    void Start () {
        this.characterClass = (int) User.Instance.CurrentCharacterInfo.Class;
        DataManager.Instance.ISkills.TryGetValue(this.characterClass, out skillMap);
        if (skillMap != null)
        {
            foreach (var kv in skillMap)
            {
                defines.Add(kv.Value);
            }
        }

        for (int i = 1; i <= 4; ++i)
        {
            UISkillSlot slot = this.transform.Find(string.Format("UISkillSlot{0}", i)).GetComponent<UISkillSlot>();
            slot.updateShow(defines[i - 1]);
            slots.Add(slot);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
