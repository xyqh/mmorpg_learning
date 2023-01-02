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
        public Creature Target;

        private float cd = 0;

        private float castTime = 0;
        private float skillTime;
        public bool IsCasting = false;
        public int Hit = 0;
        private SkillStatus Status;

        // 缓存
        private Dictionary<int, List<NDamageInfo>> HitMap = new Dictionary<int, List<NDamageInfo>>();

        private List<Bullet> Bullets = new List<Bullet>();

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
                
                int distance = this.Owner.Distance(target);
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

        public void BeginCast(Creature target)
        {
            this.IsCasting = true;
            this.castTime = 0;
            this.skillTime = 0;
            this.Hit = 0;
            this.cd = this.Define.CD;
            this.Target = target;
            this.Owner.PlayAnim(this.Define.SkillAnim);
            this.Bullets.Clear();
            this.HitMap.Clear();

            if(this.Define.CastTime > 0)
            {
                this.Status = SkillStatus.Casting;
            }
            else
            {
                this.Status = SkillStatus.Running;
            }
        }

        public void OnUpdate(float delta)
        {
            UpdateCD(delta);
            if (this.Status == SkillStatus.Casting)
            {
                this.UpdateCasting();
            }
            else if(this.Status == SkillStatus.Running)
            {
                this.UpdateSkill();
            }
        }

        private void UpdateCasting()
        {
            if (this.castTime < this.Define.CastTime)
            {
                this.castTime += Time.deltaTime;
            }
            else
            {
                this.castTime = 0;
                this.Status = SkillStatus.Running;
                Debug.LogFormat("Skill[{0}].UpdateCasting Finish", this.Define.Name);
            }
        }

        private void UpdateSkill()
        {
            this.skillTime += Time.deltaTime;
            if (this.Define.Duration > 0)
            {
                if (this.skillTime > this.Define.Interval * (this.Hit + 1))
                {
                    this.DoHit();
                }

                if (this.skillTime >= this.Define.Duration)
                {
                    this.Status = SkillStatus.None;
                    this.IsCasting = false;
                    Debug.LogFormat("Skill[{0}].UpdateSkill Finish", this.Define.Name);
                }
            }
            else if (this.Define.HitTimes != null && this.Define.HitTimes.Count > 0)
            {
                if (this.Hit < this.Define.HitTimes.Count)
                {
                    if (this.skillTime >= this.Define.HitTimes[this.Hit])
                    {
                        this.DoHit();
                    }
                }
                else
                {
                    if (!this.Define.Bullet)
                    {
                        this.Status = SkillStatus.None;
                        this.IsCasting = false;
                        Debug.LogFormat("Skill[{0}].UpdateSkill Finish", this.Define.Name);
                    }
                }
            }

            if (this.Define.Bullet)
            {
                bool finish = true;
                foreach(Bullet bullet in this.Bullets)
                {
                    bullet.Update();
                    if (!bullet.Stoped) finish = false;
                }

                if(finish && this.Hit >= this.Define.HitTimes.Count)
                {
                    this.Status = SkillStatus.None;
                    this.IsCasting = false;
                    Debug.LogFormat("Skill[{0}].UpdateSkill Finish", this.Define.Name);
                }
            }
        }

        private void DoHit()
        {
            if (this.Define.Bullet)
            {
                this.CastBullet();
            }
            else
            {
                this.DoHitDamages(this.Hit);
            }
            ++this.Hit;
        }

        public void DoHitDamages(int hit)
        {
            List<NDamageInfo> damages;
            if (this.HitMap.TryGetValue(hit, out damages))
            {
                DoHitDamages(damages);
            }
        }

        private void CastBullet()
        {
            Bullet bullet = new Bullet(this);
            Debug.LogFormat("Skill[{0}].CastBullet[{1}] Target:{2}", this.Define.Name, this.Define.BulletResource, this.Target.Name);
            this.Bullets.Add(bullet);
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

        public void DoHit(NSkillHitInfo hit)
        {
            if(hit.isBullet || !this.Define.Bullet)
            {
                this.DoHit(hit.hitId, hit.Damages);
            }
        }

        public void DoHit(int hitId, List<NDamageInfo> damages)
        {
            if (hitId <= this.Hit)
            {
                this.HitMap[hitId] = damages;
            }
            else
            {
                this.DoHitDamages(damages);
            }
        }

        private void DoHitDamages(List<NDamageInfo> damages)
        {
            foreach(var dmg in damages)
            {
                Creature target = EntityManager.Instance.GetEntity(dmg.entityId) as Creature;
                if (target == null) return;
                target.DoDamage(dmg);
            }
        }
    }
}
