using Common.Data;
using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    class ItemManager : Singleton<ItemManager>
    {
        public Dictionary<int, Item> items = new Dictionary<int, Item>();

        internal void Init(List<NItemInfo> items)
        {
            this.items.Clear();
            foreach(var info in items)
            {
                Item item = new Item(info);
                this.items.Add(item.id, item);

                Debug.LogFormat("ItemManager:Init[{0}]", item);
            }
        }

        public ItemDefine GetItem(int itemId)
        {
            return null;
        }

        public bool UseItem(int itemId)
        {
            return false;
        }

        public bool UseItem(ItemDefine item)
        {
            return false;
        }
    }
}