using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Models
{
    class User : Singleton<User>
    {
        SkillBridge.Message.NUserInfo userInfo;
        public MapDefine currentMapDef;
        private GameObject _currentCharacterObj;
        public GameObject currentCharacterObj {
            set
            {
                _currentCharacterObj = value;
            }
            get
            {
                return _currentCharacterObj;
            }
        }


        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }


        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            this.userInfo = info;
        }

        public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }

    }
}
