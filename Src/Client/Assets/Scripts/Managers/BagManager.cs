using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Models;

namespace Managers
{
    class BagManager : Singleton<BagManager>
    {
        public int unlocked;
        //public BagItem[] bagItems;
        public List<BagItem> bagItems = new List<BagItem>();
        NBagInfo info;

        unsafe public void Init(NBagInfo info)
        {
            this.info = info;
            this.unlocked = info.Unlocked;
            //this.bagItems = new BagItem[this.unlocked];
            if (info.Items != null && info.Items.Length >= this.unlocked)
            {
                Analyze(info.Items);
            }
            else
            {
                this.info.Items = new byte[sizeof(BagItem) * this.unlocked];
                Reset();
            }
        }

        public void Reset()
        {
            //int i = 0;
            foreach(var kv in ItemManager.Instance.items)
            {
                if(kv.Value.count <= kv.Value.define.StackLimit)
                {

                }
                else
                {
                    int count = kv.Value.count;
                    while(count > kv.Value.define.StackLimit)
                    {
                        BagItem _bagItem = new BagItem(kv.Key, kv.Value.define.StackLimit);
                        //bagItems[i].itemId = (ushort)kv.Key;
                        //bagItems[i].count = (ushort)kv.Value.count;
                        //++i;
                        this.bagItems.Add(_bagItem);
                        count -= kv.Value.count;
                    }
                }
                BagItem bagItem = new BagItem(kv.Key, kv.Value.count);
                this.bagItems.Add(bagItem);
                //bagItems[i].itemId = (ushort)kv.Key;
                //bagItems[i].count = (ushort)kv.Value.count;
            }
            //++i;
        }

        public void AddItem(int itemId, int count)
        {
            ushort addCount = (ushort)count;
            for(int i = 0; i < this.bagItems.Count; ++i)
            {
                if(bagItems[i].itemId == itemId)
                {
                    ushort canAdd = (ushort)(DataManager.Instance.IItems[itemId].StackLimit - this.bagItems[i].count);
                    if(canAdd >= addCount)
                    {
                        BagItem item = this.bagItems[i];
                        this.bagItems[i] = new BagItem(itemId, item.count + addCount);
                        addCount = 0;
                        break;
                    }
                    else{
                        BagItem item = this.bagItems[i];
                        this.bagItems[i] = new BagItem(itemId, item.count + canAdd);
                        addCount -= canAdd;
                    }
                }
            }
            if(addCount > 0)
            {
                for(int i = 0; i < bagItems.Count; ++i)
                {
                    if(this.bagItems[i].itemId == 0)
                    {
                        this.bagItems[i] = new BagItem(itemId, addCount);
                    }
                }
            }
        }

        public void RemoveItem(int itemId, int count)
        {
            
        }

        unsafe void Analyze(byte[] data)
        {
            fixed(byte * pt = data)
            {
                bagItems.Clear();
                for(int i = 0; i < this.unlocked; ++i)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    if (!item->Equals(BagItem.zero))
                    {
                        bagItems.Add(*item);
                    }
                    //this.bagItems[i] = *item;
                }
            }
        }

        unsafe public NBagInfo GetBagInfo()
        {
            fixed(byte* pt = this.info.Items)
            {
                for(int i = 0; i < this.unlocked && i < bagItems.Count; ++i)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    *item = this.bagItems[i];
                }
            }
            return this.info;
        }
    }
}