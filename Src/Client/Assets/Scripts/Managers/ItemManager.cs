using Common.Data;
using Models;
using Services;
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
            StatusService.Instance.RegisterStatusNotify(StatusType.Item, this.OnItemNotify);
        }

        private bool OnItemNotify(NStatus status)
        {
            if(status.Action == StatusAction.Add)
            {
                this.AddItem(status.Id, status.Value);
            }
            if (status.Action == StatusAction.Delete)
            {
                this.RemoveItem(status.Id, status.Value);
            }

            return true;
        }

        private void AddItem(int itemId, int count)
        {
            Item item = null;
            if(items.TryGetValue(itemId, out item))
            {
                items[itemId].count += count;
            }
            else
            {
                item = new Item(itemId, count);
                items[itemId] = item;
            }
            BagManager.Instance.AddItem(itemId, count);
        }

        private void RemoveItem(int itemId, int count)
        {
            if (!this.items.ContainsKey(itemId))
            {
                return;
            }
            Item item = this.items[itemId];
            if(item.count < count)
            {
                return;
            }
            item.count -= count;

            BagManager.Instance.RemoveItem(itemId, count);
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