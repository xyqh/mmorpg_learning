using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameServer.Models
{
    class Item
    {
        TCharacterItem dbItem;
        public int itemId;
        public int count;

        public Item(TCharacterItem item)
        {
            dbItem = item;
            itemId = (short)item.ItemID;
            count = (short)item.ItemCount;
        }

        public void Add(int count)
        {
            this.count += count;
            dbItem.ItemCount = count;
        }

        public void Remove(int count)
        {
            this.count -= count;
            dbItem.ItemCount = count;
        }

        public bool Use(int count = 1)
        {
            return false;
        }

        public override string ToString()
        {
            return string.Format("ID:{0},Count:{1}", itemId, count);
        }
    }
}