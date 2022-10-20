using Common;
using Common.Data;
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
    class Character : CharacterBase
    {
       
        public TCharacter Data;
        public ItemManager ItemManager;
        public StatusManager StatusManager;

        public Character(CharacterType type,TCharacter cha):
            base(new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ),new Core.Vector3Int(100,0,0))
        {
            this.Data = cha;
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Id = cha.ID;
            this.Info.Name = cha.Name;
            this.Info.Level = 1;//cha.Level;
            this.Info.Exp = cha.Exp;
            this.Info.ConfigId = cha.TID;
            this.Info.Class = (CharacterClass)cha.Class;
            this.Info.mapId = cha.MapID;
            this.Info.Gold = cha.Gold;
            this.Info.Entity = EntityData;
            this.Info.Equips = cha.Equips;
            this.Define = DataManager.Instance.Characters[Info.ConfigId];

            ItemManager = new ItemManager(this);
            ItemManager.GetItemInfos(Info.Items);
            this.Info.Bag = new NBagInfo();
            this.Info.Bag.Unlocked = this.Data.Bag.Unlocked;
            this.Info.Bag.Items = this.Data.Bag.Items;
            this.StatusManager = new StatusManager(this);

            this.Info.attrDynamic = new NAttributeDynamic();
            this.Info.attrDynamic.Hp = cha.HP;
            this.Info.attrDynamic.Mp = cha.MP;
        }

        internal void AddExp(int exp)
        {
            this.Exp += exp;
            this.CheckLevelUp();
        }

        void CheckLevelUp()
        {
            long needExp = (long)Math.Pow(this.Level, 3) * 10 + this.Level * 40 + 50;
            if(this.Exp > needExp)
            {
                this.LevelUp();
            }
        }

        void LevelUp()
        {
            this.Level += 1;
            Log.InfoFormat("");
            CheckLevelUp();
        }

        public long Gold
        {
            get { return this.Data.Gold; }
            set
            {
                if (this.Data.Gold == value) return;
                this.StatusManager.AddGoldChange((int)(value - this.Data.Gold));
                this.Data.Gold = value;
            }
        }

        public long Exp
        {
            get { return this.Data.Exp; }
            private set
            {
                if(this.Data.Exp == value)
                {
                    return;
                }
                this.StatusManager.addExpChange((int)(value - this.Data.Exp));
                this.Data.Exp = value;
            }
        }

        public int Level
        {
            get { return this.Data.Level; }
            private set
            {
                if(this.Data.Level == value)
                {
                    return;
                }
                this.StatusManager.addLevelUp((int)(value - this.Data.Level));
                this.Data.Level = value;
            }
        }
    }
}
