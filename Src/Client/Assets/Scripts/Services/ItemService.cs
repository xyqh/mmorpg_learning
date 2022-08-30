using Common.Data;
using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Services
{
    class ItemService : Singleton<ItemService>, IDisposable
    {

        public ItemService()
        {
            MessageDistributer.Instance.Subscribe<ItemBuyResponse>(this.OnItemBuy);
            MessageDistributer.Instance.Subscribe<ItemEquipResponse>(this.OnItemEquip);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(this.OnItemBuy);
            MessageDistributer.Instance.Unsubscribe<ItemEquipResponse>(this.OnItemEquip);
        }

        public void Init()
        {

        }

        public void SendItemBuy(int shopId, int shopItemId)
        {
            Debug.LogFormat("ItemService:SendItemBuy {0}, {1}", shopId, shopItemId);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemBuy = new ItemBuyRequest();
            message.Request.itemBuy.shopId = shopId;
            message.Request.itemBuy.shopItemId = shopItemId;
            NetClient.Instance.SendMessage(message);
        }

        private void OnItemBuy(object sender, ItemBuyResponse response)
        {
            MessageBox.Show("购买结果" + response.Result + "\n" + response.Errormsg, "购买完成");
        }

        Item pendingEquip = null;
        bool isEquip;
        public bool SendEquipItem(Item equip, bool isEquip)
        {
            if(pendingEquip != null)
            {
                return false;
            }

            pendingEquip = equip;
            this.isEquip = isEquip;

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemEquip = new ItemEquipRequest();
            message.Request.itemEquip.Slot = (int)equip.equipDefine.slot;
            message.Request.itemEquip.itemId = equip.id;
            message.Request.itemEquip.isEquip = isEquip;
            NetClient.Instance.SendMessage(message);

            return true;
        }

        private void OnItemEquip(object sender, ItemEquipResponse response)
        {
            if(response.Result == Result.Success)
            {
                if(pendingEquip != null)
                {
                    if (this.isEquip)
                    {
                        EquipManager.Instance.OnEquipItem(pendingEquip);
                    }
                    else
                    {
                        EquipManager.Instance.OnUnEquipItem(pendingEquip.equipDefine.slot);
                    }
                    pendingEquip = null;
                }
            }
        }
    }
}
