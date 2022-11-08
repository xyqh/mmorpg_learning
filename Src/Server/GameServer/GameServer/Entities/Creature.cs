using Common.Battle;
using Common.Data;
using GameServer.Battle;
using GameServer.Core;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Entities
{
    class Creature : Entity
    {

        public int Id { get; set; }
        public NCharacterInfo Info;
        public CharacterDefine Define;

        public Attributes Attributes;
        public SkillManager SkillMgr;

        public bool IsDeath = false;

        public Creature(CharacterType type, int configId, int level, Vector3Int pos, Vector3Int dir) :
           base(pos, dir)
        {
            this.Define = DataManager.Instance.Characters[configId];

            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Level = level;
            this.Info.ConfigId = configId;
            this.Info.Entity = this.EntityData;
            this.Info.EntityId = this.entityId;
            this.Info.Name = this.Define.Name;

            this.InitSkills();

            this.Attributes = new Attributes();
            this.Attributes.Init(this.Define, this.Info.Level, this.GetEquips(), this.Info.attrDynamic);
            this.Info.attrDynamic = this.Attributes.DynamicAttr;
        }

        public int Distance(Creature target)
        {
            return (int)Vector3Int.Distance(this.Position, target.Position);
        }

        public int Distance(Vector3Int position)
        {
            return (int)Vector3Int.Distance(this.Position, position);
        }

        public void DoDamage(NDamageInfo damage)
        {
            this.Attributes.HP -= damage.Damage;
            if(this.Attributes.HP <= 0)
            {
                this.IsDeath = true;
                damage.WillDead = true;
            }
        }

        void InitSkills()
        {
            this.SkillMgr = new SkillManager(this);
            this.Info.Skills.AddRange(this.SkillMgr.Infos);
        }

        public virtual List<EquipDefine> GetEquips()
        {
            return null;
        }

        public void CastSkill(BattleContext context, int skillId)
        {
            Skill skill = this.SkillMgr.GetSkill(skillId);
            if(skill != null)
            {
                context.Result = skill.Cast(context);
            }
        }

        public override void Update()
        {
            this.SkillMgr.Update();
        }
    }
}
