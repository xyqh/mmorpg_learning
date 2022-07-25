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

        public Item(NItemInfo item)
        {
            id = item.Id;
            count = item.Count;
        }

        public override string ToString()
        {
            return string.Format("Id:{0}, Count:{1}", id, count);
        }
    }
}
