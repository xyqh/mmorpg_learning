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

        private float cd = 0;

        public NDamageInfo Damage { get; private set; }

        private float castTime = 0;
        private float skillTime;
        public bool IsCasting = false;
        private int hit;

        public float CD
        {
            get { return cd; }
        }

        public Skill(NSkillInfo info, Creature owner)
        {
            this.Info = info;
            this.Owner = owner;
            this.Define = DataManager.Instance.ISkills[this.Owner.Define.TID][this.Info.Id];
            this.cd = 0;
        }

        public SkillResult CanCast(Creature target)
        {
            if (this.Define.CastTarget == TargetType.Target)
            {
                if (target == null || target == this.Owner)
                    return SkillResult.InvalidTarget;

                int distance = (int)Vector3Int.Distance(this.Owner.position, target.position);
                //int distance = (int)Vector3Int.Distance(this.Owner.position, target.position) - this.Owner.Define.Radius - target.Define.Radius;
                if (distance > this.Define.CastRange)
                {
                    return SkillResult.OutOfRange;
                }
            }

            if (this.Define.CastTarget == TargetType.Position && BattleManager.Instance.CurrentPosition == null)
            {
                return SkillResult.InvalidPosition;
            }

            if (this.Owner.Attributes.MP < this.Define.MPCost)
            {
                return SkillResult.LackOfMp;
            }

            if(this.cd > 0)
            {
                return SkillResult.CoolDown;
            }

            return SkillResult.Ok;
        }

        public void BeginCast(NDamageInfo damage)
        {
            this.IsCasting = true;
            this.castTime = 0;
            this.skillTime = 0;
            this.cd = this.Define.CD;
            this.Damage = damage;

            this.Owner.PlayAnim(this.Define.SkillAnim);
        }

        public void OnUpdate(float delta)
        {
            if (this.IsCasting)
            {
                this.skillTime += delta;
                if(this.skillTime > 0.5f && this.hit == 0)
                //if(this.skillTime >= this.Define.HitTime && this.hit == 0)
                {
                    this.DoHit();
                }
                if(this.skillTime >= this.Define.CD)
                {
                    this.skillTime = 0;
                }
            }

            UpdateCD(delta);
        }

        private void DoHit()
        {
            ++this.hit;
            if(this.Damage != null)
            {
                var cha = CharacterManager.Instance.GetCharacter(this.Damage.entityId);
                cha.DoDamage(this.Damage);
            }
        }

        private void UpdateCD(float delta)
        {
            if(this.cd > 0)
            {
                this.cd -= delta;
            }
            if(this.cd < 0)
            {
                this.cd = 0;
            }
        }
    }
}
