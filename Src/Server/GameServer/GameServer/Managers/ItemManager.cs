using Common;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameServer.Managers
{
    class ItemManager
    {
        Character owner;
        public Dictionary<int, Item> items = new Dictionary<int, Item>();

        public ItemManager(Character owner)
        {
            this.owner = owner;
            foreach(var item in owner.Data.Items)
            {
                this.items.Add(item.ItemID, new Item(item));
            }
        }

        public bool UseItem(int itemId, int count = 1)
        {
            Log.InfoFormat("[{0}]UseItem[{1}:{2}]", owner.Data.ID, itemId, count);
            Item item = null;
            if(items.TryGetValue(itemId, out item))
            {
                if(item.count < count)
                {
                    return false;
                }

                // 使用逻辑
                item.Remove(count);

                return true;
            }

            return false;
        }

        public bool HasItem(int itemId)
        {
            Item item = null;
            if (items.TryGetValue(itemId, out item))
            {
                return item.count > 0;
            }
            return false;
        }

        public Item GetItem(int itemId)
        {
            Item item = null;
            items.TryGetValue(itemId, out item);
            Log.InfoFormat("[{0}]GetItem[{1}:{2}]", owner.Data.ID, itemId, item);
            return item;
        }

        public bool AddItem(int itemId, int count)
        {
            Item item = null;
            if(items.TryGetValue(itemId, out item))
            {
                item.Add(count);
            }
            else
            {
                TCharacterItem dbItem = new TCharacterItem();
                dbItem.CharacterID = owner.Data.ID;
                dbItem.Owner = owner.Data;
                dbItem.ItemID = itemId;
                dbItem.Id = itemId;
                dbItem.ItemCount = count;
                owner.Data.Items.Add(dbItem);
                item = new Item(dbItem);
                items.Add(itemId, item);
            }
            Log.InfoFormat("[{0}]AddItem[{1}] addCount:{2}", owner.Data.ID, item, count);
            DBService.Instance.Save();

            return true;
        }

        public bool RemoveItem(int itemId, int count)
        {
            if (!items.ContainsKey(itemId))
            {
                return false;
            }

            Item item = items[itemId];
            if(item.count < count)
            {
                return false;
            }
            item.Remove(count);
            Log.InfoFormat("[{0}]RemoveItem[{1}] removeCount:{2}", owner.Data.ID, item, count);
            DBService.Instance.Save();
            return true;
        }

        public void GetItemInfos(List<NItemInfo> list)
        {
            foreach(var item in items)
            {
                list.Add(new NItemInfo() { Id = item.Value.itemId, Count = item.Value.count });
            }
        }
    }
}