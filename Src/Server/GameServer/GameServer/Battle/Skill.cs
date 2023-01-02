using Battle;
using Common;
using Common.Data;
using Common.Utils;
using GameServer.Core;
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

        public SkillStatus Status;

        private float cd = 0;
        public float CD
        {
            get { return cd; }
        }
        private float castingTime = 0;
        private float skillTime = 0;
        private int Hit = 0;
        BattleContext Context;
        //NSkillHitInfo HitInfo;
        List<Bullet> Bullets = new List<Bullet>();

        public bool Instant
        {
            get
            {
                if (this.Define.CastTime > 0) return false;
                if (this.Define.Bullet) return false;
                if (this.Define.Duration > 0) return false;
                if (this.Define.HitTimes != null && this.Define.HitTimes.Count > 0) return false;
                return true;
            }
        }

        public Skill(NSkillInfo info, Creature owner)
        {
            this.Info = info;
            this.Owner = owner;
            this.Define = DataManager.Instance.Skills[this.Owner.Define.TID][this.Info.Id];
        }

        public SkillResult CanCast(BattleContext context)
        {
            if(this.Status != SkillStatus.None)
            {
                return SkillResult.Casting;
            }

            if(this.Define.CastTarget == Common.Battle.TargetType.Target)
            {
                if(context.Target == null || context.Target == this.Owner)
                {
                    return SkillResult.InvalidTarget;
                }

                int distance = this.Owner.Distance(context.Target);
                if(distance > this.Define.CastRange)
                {
                    return SkillResult.OutOfRange;
                }
            }

            if(this.Define.CastTarget == Common.Battle.TargetType.Position)
            {
                if(context.CastSkill.Position == null)
                {
                    return SkillResult.InvalidTarget;
                }
                if(this.Owner.Distance(context.Position) > this.Define.CastRange)
                {
                    return SkillResult.OutOfRange;
                }
            }

            if(this.Owner.Attributes.MP < this.Define.MPCost)
            {
                return SkillResult.LackOfMp;
            }

            if(this.cd > 0)
            {
                return SkillResult.CoolDown;
            }

            return SkillResult.Ok;
        }

        public SkillResult Cast(BattleContext context)
        {
            SkillResult result = this.CanCast(context);
            if(result == SkillResult.Ok)
            {
                // 重置数据
                this.castingTime = 0;
                this.skillTime = 0;
                this.cd = this.Define.CD;
                this.Context = context;
                this.Hit = 0;
                this.Bullets.Clear();

                if (this.Instant)
                {
                    this.DoHit();
                }
                else
                {
                    if(this.Define.CastTime > 0)
                    {
                        this.Status = SkillStatus.Casting;
                    }
                    else
                    {
                        this.Status = SkillStatus.Running;
                    }
                }
            }
            Log.InfoFormat("Skill[{0}].Cast Result:[{1}] Status:{2}", this.Define.Name, result, this.Status);

            return result;
        }

        private NSkillHitInfo InitHitInfo(bool isBullet)
        {
            NSkillHitInfo hitInfo = new NSkillHitInfo();
            hitInfo.casterId = this.Context.Caster.entityId;
            hitInfo.skillId = this.Info.Id;
            hitInfo.hitId = this.Hit;
            hitInfo.isBullet = isBullet;
            return hitInfo;
        }

        private void DoHit()
        {
            NSkillHitInfo hitInfo = this.InitHitInfo(false);

            Log.InfoFormat("Skill[{0}].DoHit[{1}]", this.Define.Name, this.Hit);
            ++this.Hit;

            if (this.Define.Bullet)
            {
                this.CastBullet(hitInfo);
                return;
            }

            DoHit(hitInfo);
        }

        public void DoHit(NSkillHitInfo hitInfo)
        {
            Context.Battle.AddHitInfo(hitInfo);
            Log.InfoFormat("Skill[{0}].DoHit[{1}] IsBullet:{2}", this.Define.Name, hitInfo.hitId, hitInfo.isBullet);

            if(this.Define.AOERange > 0)
            {
                this.HitRange(hitInfo);
                return;
            }
            if(this.Define.CastTarget == Common.Battle.TargetType.Target)
            {
                this.HitTarget(Context.Target, hitInfo);
                return;
            }
        }

        private void CastBullet(NSkillHitInfo hitInfo)
        {
            Context.Battle.AddHitInfo(hitInfo);
            Log.InfoFormat("Skill[{0}].CastBullet[{1}] Target:{2}", this.Define.Name, this.Define.BulletResource, this.Context.Target.Define.Name);

            Bullet bullet = new Bullet(this, this.Context.Target, hitInfo);
            this.Bullets.Add(bullet);
        }

        private void HitRange(NSkillHitInfo hitInfo)
        {
            Vector3Int pos;
            if(this.Define.CastTarget == Common.Battle.TargetType.Target)
            {
                pos = Context.Target.Position;
            }
            else if(this.Define.CastTarget == Common.Battle.TargetType.Position)
            {
                pos = Context.Position;
            }
            else
            {
                pos = this.Owner.Position;
            }

            List<Creature> units = this.Context.Battle.FindUnitsInRange(pos, this.Define.AOERange);
            foreach(var target in units)
            {
                this.HitTarget(target, hitInfo);
            }
        }

        private void HitTarget(Creature target, NSkillHitInfo hitInfo)
        {
            if (this.Define.CastTarget == Common.Battle.TargetType.Self && (target != Context.Caster))
            {
                return;
            }
            else if(target == Context.Caster)
            {
                return;
            }

            NDamageInfo damage = this.CalcSkillDamage(Context.Caster, target);
            Log.InfoFormat("Skill[{0}].HitTarget[{1}] Damage:{2} Crit:{3}", this.Define.Name, target.Define.Name, damage.Damage, damage.Crit);
            target.DoDamage(damage);
            hitInfo.Damages.Add(damage);
        }

        private NDamageInfo CalcSkillDamage(Creature caster, Creature target)
        {
            float ad = this.Define.AD + caster.Attributes.AD * this.Define.ADFactor;
            float ap = this.Define.AP + caster.Attributes.AP * this.Define.APFactor;

            float adDmg = ad * (1 - target.Attributes.DEF / (target.Attributes.DEF + 100));
            float apDmg = ap * (1 - target.Attributes.MDEF / (target.Attributes.MDEF + 100));

            float final = adDmg + apDmg;
            bool isCrit = this.IsCrit(caster.Attributes.CRI);
            if (isCrit)
            {
                final = final * 2f;
            }

            final *= (float)MathUtil.Random.NextDouble() * 0.1f - 0.05f;

            NDamageInfo damage = new NDamageInfo();
            damage.entityId = target.entityId;
            damage.Damage = Math.Max(1, (int)final);
            damage.Crit = isCrit;

            return damage;
        }

        private bool IsCrit(float crit)
        {
            return MathUtil.Random.NextDouble() < crit;
        }

        public void Update()
        {
            UpdateCD();
            if(this.Status == SkillStatus.Casting)
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
            if(this.castingTime < this.Define.CastTime)
            {
                this.castingTime += Time.deltaTime;
            }
            else
            {
                this.castingTime = 0;
                this.Status = SkillStatus.Running;
                Log.InfoFormat("Skill[{0}].UpdateCasting Finish", this.Define.Name);
            }
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

        private void UpdateSkill()
        {
            this.skillTime += Time.deltaTime;
            if(this.Define.Duration > 0)
            {
                if(this.skillTime > this.Define.Interval * (this.Hit + 1))
                {
                    this.DoHit();
                }

                if(this.skillTime >= this.Define.Duration)
                {
                    this.Status = SkillStatus.None;
                    Log.InfoFormat("Skill[{0}].UpdateSkill Finish", this.Define.Name);
                }
            }
            else if(this.Define.HitTimes != null && this.Define.HitTimes.Count > 0)
            {
                if(this.Hit < this.Define.HitTimes.Count)
                {
                    if(this.skillTime >= this.Define.HitTimes[this.Hit])
                    {
                        this.DoHit();
                    }
                }
                else
                {
                    if (!this.Define.Bullet)
                    {
                        this.Status = SkillStatus.None;
                        Log.InfoFormat("Skill[{0}].UpdateSkill Finish", this.Define.Name);
                    }
                }
            }

            if (this.Define.Bullet)
            {
                bool finish = true;
                foreach(var bullet in this.Bullets)
                {
                    bullet.Update();
                    if (!bullet.Stoped) finish = false;
                }

                if(finish && this.Hit >= this.Define.HitTimes.Count)
                {
                    this.Status = SkillStatus.None;
                    Log.InfoFormat("Skill[{0}].UpdateSkill Bullet Finish", this.Define.Name);
                }
            }
        }
    }
}