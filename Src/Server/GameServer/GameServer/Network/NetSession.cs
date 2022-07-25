﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameServer;
using GameServer.Entities;
using GameServer.Services;
using SkillBridge.Message;

namespace Network
{
    class NetSession
    {
        public TUser User { get; set; }
        public Character Character { get; set; }
        public NEntity Entity { get; set; }

        internal void DisConnected()
        {
            if(Character != null)
            {
                UserService.Instance.CharacterLeave(Character);
            }
        }
    }
}
