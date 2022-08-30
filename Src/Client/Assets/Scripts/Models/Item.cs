using Common.Data;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{

    public class Item
    {
        public int id;
        public int count;
        public EquipDefine equipDefine;
        public ItemDefine define;

        public Item(NItemInfo item)
        {
            this.id = item.Id;
            this.count = item.Count;
            DataManager.Instance.IItems.TryGetValue(item.Id, out this.define);
            DataManager.Instance.IEquips.TryGetValue(item.Id, out this.equipDefine);
        }

        public Item(int id, int count)
        {
            this.id = id;
            this.count = count;
            DataManager.Instance.IItems.TryGetValue(id, out this.define);
            DataManager.Instance.IEquips.TryGetValue(id, out this.equipDefine);
        }

        public override string ToString()
        {
            return string.Format("Id:{0}, Count:{1}", id, count);
        }
    }
}
