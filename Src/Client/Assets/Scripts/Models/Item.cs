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
        public ItemDefine define;

        public Item(NItemInfo item)
        {
            this.id = item.Id;
            this.count = item.Count;
            this.define = DataManager.Instance.IItems[item.Id];
        }

        public Item(int id, int count)
        {
            this.id = id;
            this.count = count;
            this.define = DataManager.Instance.IItems[id];
        }

        public override string ToString()
        {
            return string.Format("Id:{0}, Count:{1}", id, count);
        }
    }
}
