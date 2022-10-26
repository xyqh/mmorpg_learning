using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common.Data;
using Managers;
using SkillBridge.Message;

namespace Entities
{
    public class Character : Creature
    {
        public Character(NCharacterInfo info) : base(info)
        {
        }

        public override List<EquipDefine> GetEquips()
        {
            return EquipManager.Instance.GetEquipedDefines();
        }
    }
}