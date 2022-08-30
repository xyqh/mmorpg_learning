using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Managers
{
    class EquipManager : Singleton<EquipManager>
    {
        public delegate void OnEquipChangeHandler();
        public event OnEquipChangeHandler OnEquipChanged;
        public Item[] equips = new Item[(int)EquipSlot.SlotMax];
        byte[] data; 

        unsafe public void Init(byte[] data)
        {
            this.data = data;
            this.ParseEquipData(data);
        }

        public bool Contains(int equipId)
        {
            for(int i = 0; i < this.equips.Length; ++i)
            {
                if(equips[i] != null && equips[i].id == equipId)
                {
                    return true;
                }
            }
            return false;
        }

        public Item GetEquip(EquipSlot slot)
        {
            return this.equips[(int)slot];
        }

        unsafe void ParseEquipData(byte[] data)
        {
            fixed(byte* pt = this.data)
            {
                for(int i = 0; i < this.equips.Length; ++i)
                {
                    int itemId = *(int*)(pt + i * sizeof(int));
                    if(itemId > 0)
                    {
                        this.equips[i] = ItemManager.Instance.items[itemId];
                    }
                    else
                    {
                        equips[i] = null;
                    }
                }
            }
        }

        unsafe public byte[] GetEquipData()
        {
            fixed(byte* pt = this.data)
            {
                for(int i = 0; i < (int)EquipSlot.SlotMax; ++i)
                {
                    int* itemId = (int*)(pt + i * sizeof(int));
                    if(this.equips[i] == null)
                    {
                        *itemId = -1;
                    }
                    else
                    {
                        *itemId = this.equips[i].id;
                    }
                }
            }
        }

        public void EquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip, true);
        }

        public void UnEquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip, false);
        }

        public void OnEquipItem(Item equip)
        {
            if(this.equips[(int)equip.equipDefine.slot] != null && this.equips[(int)equip.equipDefine.slot].id == equip.id)
            {
                return;
            }
            this.equips[(int)equip.equipDefine.slot] = ItemManager.Instance.items[equip.id];

            if(OnEquipChanged != null)
            {
                OnEquipChanged();
            }
        }

        public void OnUnEquipItem(EquipSlot slot)
        {
            if(this.equips[(int)slot] != null)
            {
                this.equips[(int)slot] = null;
                if(OnEquipChanged != null)
                {
                    OnEquipChanged();
                }
            }
        }
    }
}