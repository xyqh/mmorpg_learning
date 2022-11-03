using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameServer.Battle
{
    class Skill
    {
        public NSkillInfo Info;
        public Creature Owner;
        public SkillDefine Define;

        private float cd = 0;
        public float CD
        {
            get { return cd; }
        }

        public Skill(NSkillInfo info, Creature owner)
        {
            this.Info = info;
            this.Owner = owner;
            this.Define = DataManager.Instance.Skills[this.Owner.Define.TID][this.Info.Id];
        }

        public SkillResult Cast(BattleContext context)
        {
            SkillResult result = SkillResult.Ok;
            if(this.cd > 0)
            {
                result = SkillResult.CoolDown;
            }

            if(context.Target != null)
            {
                this.DoSkillDamage(context);
            }
            //else
            //{
            //    result = SkillResult.InvalidTarget;
            //}

            if(result == SkillResult.Ok)
            {
                this.cd = this.Define.CD;
            }

            return result;
        }

        private void DoSkillDamage(BattleContext context)
        {
            context.Damage = new NDamageInfo();
            context.Damage.entityId = context.Target.entityId;
            context.Damage.Damage = 100;
            context.Target.DoDamage(context.Damage);
        }

        public void Update()
        {
            UpdateCD();
        }

        private void UpdateCD()
        {
            if(this.cd > 0)
            {
                this.cd -= Time.deltaTime;
            }
            if(this.cd < 0)
            {
                this.cd = 0;
            }
        }
    }
}