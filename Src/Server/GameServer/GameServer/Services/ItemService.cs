using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameServer.Services
{
    class ItemService : Singleton<ItemService>
    {
        public ItemService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemBuyRequest>(this.OnItemBuy);
        }

        public void Init()
        {

        }

        private void OnItemBuy(NetConnection<NetSession> sender, ItemBuyRequest request)
        {
            //NetMessage message = new NetMessage();
            //message.Response = new NetMessageResponse();

            //message.Response.mapEntitySync = new MapEntitySyncResponse();
            //message.Response.mapEntitySync.entitySyncs.Add(entitySync);

            //byte[] data = PackageHandler.PackMessage(message);
            //connection.SendData(data, 0, data.Length);

            Character character = sender.Session.Character;
            Log.InfoFormat("OnItemBuy::character:{0}:Shop:{1}:ShopItem:{2}", character.Id, request.shopId, request.shopItemId);
            var result = ShopManager.Instance.BuyItem(sender, request.shopId, request.shopItemId);
            sender.Session.Response.itemBuy = new ItemBuyResponse();
            sender.Session.Response.itemBuy.Result = result;
            sender.Session.Response.itemBuy.Errormsg = "";
            sender.SendResponse();
        }
    }
}