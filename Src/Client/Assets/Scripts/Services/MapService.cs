using Common.Data;
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
        private int currentMapId;

        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);

        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
        }

        void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("OnMapCharacterEnter:{0}", response.mapId);
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
                EnterMap(response.mapId);
                currentMapId = response.mapId;
            }
        }

        void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {

        }

        void EnterMap(int mapId)
        {
            if (DataManager.Instance.IMaps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.IMaps[mapId];
                SceneManager.Instance.LoadScene(map.Resource);
            }
            else
            {
                Debug.LogFormat("EnterMap: Map {0} not existed", mapId);
            }
        }

        public void Init()
        {

        }
    }
}
