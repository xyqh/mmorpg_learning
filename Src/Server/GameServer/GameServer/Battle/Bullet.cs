using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common;
using GameServer.Battle;
using GameServer.Entities;
using SkillBridge.Message;

namespace Battle
{
    class Bullet
    {
        private Skill skill;
        private Creature target;
        private NSkillHitInfo hitInfo;
        bool TimeMode = true;
        float duration = 0;
        float flyTime = 0;

        public bool Stoped = false;

        public Bullet(Skill skill, Creature target, NSkillHitInfo hitInfo)
        {
            this.skill = skill;
            this.target = target;
            this.hitInfo = hitInfo;
            int distance = skill.Owner.Distance(target);
            if (TimeMode)
            {
                duration = distance / this.skill.Define.BulletSpeed;
            }
            Log.InfoFormat("Bullet[{0}].CastBullet[{1}] Target:{2} Distance:{3} Time:{4}", this.skill.Define.Name, this.skill.Define.BulletResource, target.Define.Name, distance, duration);
        }

        public void Update()
        {
            if (TimeMode)
            {
                this.UpdateTime();
            }
            else
            {
                this.UpdatePos();
            }
        }

        private void UpdateTime()
        {
            this.flyTime += Time.deltaTime;
            if(this.flyTime >= this.duration)
            {
                this.hitInfo.isBullet = true;
                this.skill.DoHit(this.hitInfo);
                this.Stoped = true;
            }
        }

        private void UpdatePos()
        {
            //int distance = skill.Owner.Distance(this.target);
            //if(distance > 50)
            //{
            //    pos += speed * Time.deltaTime;
            //}
            //else
            //{
            //    this.hitInfo.isBullet = true;
            //    this.skill.DoHit(this.hitInfo);
            //    this.stoped = true;
            //}
        }
    }
}