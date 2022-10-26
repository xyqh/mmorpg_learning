using Common.Battle;
using Common.Data;
using Entities;
using Managers;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Skill
    {
        public NSkillInfo Info;
        public Creature Owner;
        public SkillDefine Define;
        public float CD = 0;

        public Skill(NSkillInfo info, Creature owner)
        {
            this.Info = info;
            this.Owner = owner;
            this.Define = DataManager.Instance.ISkills[this.Owner.Define.TID][this.Info.Id];
        }

        public SkillResult CanCast()
        {
            if(this.Define.CastTarget == TargetType.Target && BattleManager.Instance.Target == null)
            {
                return SkillResult.InvalidTarget;
            }

            if(this.Define.CastTarget == TargetType.Position && BattleManager.Instance.Position == Vector3.negativeInfinity)
            {
                return SkillResult.InvalidPosition;
            }

            if(this.Owner.Attributes.MP < this.Define.MPCost)
            {
                return SkillResult.LackOfMP;
            }

            if(this.CD > 0)
            {
                return SkillResult.CoolDown;
            }

            return SkillResult.OK;
        }

        internal void Cast()
        {
            throw new NotImplementedException();
        }
    }
}
