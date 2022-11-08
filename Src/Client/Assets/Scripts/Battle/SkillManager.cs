using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class SkillManager
    {
        public delegate void SkillInfoUpdateHandle();
        public event SkillInfoUpdateHandle OnSkillInfoUpdate;

        Creature Owner;
        public List<Skill> Skills { get; private set; }

	    public SkillManager(Creature owner)
        {
            this.Owner = owner;
            this.Skills = new List<Skill>();
            this.InitSkills();
        }

        public void InitSkills(Creature owner = null)
        {
            if (owner != null)
            {
                this.Owner = owner;
            }

            this.Skills.Clear();
            foreach(var skillInfo in this.Owner.Info.Skills)
            {
                Skill skill = new Skill(skillInfo, this.Owner);
                this.AddSkill(skill);
            }
            if(OnSkillInfoUpdate != null)
            {
                OnSkillInfoUpdate();
            }
        }

        public void UpdateSkills()
        {
            foreach(var skillInfo in this.Owner.Info.Skills)
            {
                Skill skill = this.GetSkill(skillInfo.Id);
                if(skill != null)
                {
                    skill.Info = skillInfo;
                }
                else
                {
                    skill = new Skill(skillInfo, this.Owner);
                    this.AddSkill(skill);
                }
            }
            if (OnSkillInfoUpdate != null)
            {
                OnSkillInfoUpdate();
            }
        }

        public void AddSkill(Skill skill)
        {
            this.Skills.Add(skill);
        }

        public Skill GetSkill(int skillId)
        {
            for(int i = 0; i < this.Skills.Count; ++i)
            {
                if(this.Skills[i].Define.ID == skillId)
                {
                    return this.Skills[i];
                }
            }
            return null;
        }

        public void OnUpdate(float delta)
        {
            for(int i = 0; i < this.Skills.Count; ++i)
            {
                this.Skills[i].OnUpdate(delta);
            }
        }
    }
}
