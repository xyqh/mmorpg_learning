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
    class MapService : Singleton<MapService>, IDisposable
    {
        public int currentMapId;

        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(this.OnMapEntitySync);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Unsubscribe<MapEntitySyncResponse>(this.OnMapEntitySync);
        }

        void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("OnMapCharacterEnter:{0}, {1}", response.mapId, currentMapId);
            foreach(var cha in response.Characters)
            {
                if(User.Instance.CurrentCharacter.Id == cha.Id)
                {
                    User.Instance.CurrentCharacter = cha;
                }
                CharacterManager.Instance.AddCharacter(cha);
            }
            if(currentMapId != response.mapId)
            {
                currentMapId = response.mapId;
                EnterMap(response.mapId);
            }
        }

        void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {
            Debug.LogFormat("OnMapCharacterLeave:{0}", response.characterId);
            if(response.characterId != User.Instance.CurrentCharacter.Id)
            {
                CharacterManager.Instance.RemoveCharacter(response.characterId);
            }
            else
            {
                CharacterManager.Instance.Clear();
            }
        }

        void EnterMap(int mapId)
        {
            if (DataManager.Instance.IMaps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.IMaps[mapId];
                User.Instance.currentMapDef = map;
                SceneManager.Instance.LoadScene(map.Resource);
            }
            else
            {
                Debug.LogFormat("EnterMap: Map {0} not existed", mapId);
            }
        }

        public void SendMapEntitySync(EntityEvent entityEvent, NEntity entity)
        {
            Debug.LogFormat("MapEntitySyncRequest :ID:{0} POS:{1} DIR:{2} SPD:{3} ", entity.Id, entity.Position.ToString(), entity.Direction.String(), entity.Speed);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapEntitySync = new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync()
            {
                Id = entity.Id,
                Event = entityEvent,
                Entity = entity
            };
            NetClient.Instance.SendMessage(message);
        }

        private void OnMapEntitySync(object sender, MapEntitySyncResponse response)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("MapEntityUpdateResponse:Entitys:{0}", response.entitySyncs.Count);
            sb.AppendLine();
            foreach(var entity in response.entitySyncs)
            {
                EntityManager.Instance.OnEntitySync(entity);
                sb.AppendFormat("   [{0}]evt:{1} entity:{2}", entity.Id, entity.Event, entity.Entity.String());
                sb.AppendLine();
            }
            Debug.Log(sb.ToString());
        }

        public void Init()
        {

        }
    }
}
