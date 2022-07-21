using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using UnityEngine;

namespace Managers
{
    class MiniMapManager : Singleton<MiniMapManager>
    {
        public Transform PlayerTransform
        {
            get
            {
                if (User.Instance.CurrentCharacter == null)
                    return null;
                return User.Instance.currentCharacterObj.transform;
            }
        }
        private Dictionary<int, Sprite> mapSprite = new Dictionary<int, Sprite>();

        public Sprite LoadCurrentMiniMap(){
            int mapId = User.Instance.currentMapDef.ID;
            if (!mapSprite.ContainsKey(mapId))
            {
                mapSprite[mapId] = Resloader.Load<Sprite>(User.Instance.currentMapDef.MiniMap);
            }
            return mapSprite[mapId];
        }
    }
}
