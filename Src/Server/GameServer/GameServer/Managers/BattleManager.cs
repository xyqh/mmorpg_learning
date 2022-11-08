using Common;
using GameServer.Entities;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameServer.Managers
{
    class BattleManager : Singleton<BattleManager>
    {
        static long bid = 0;

        public void Init()
        {

        }

        public void ProcessBattleMessage(NetConnection<NetSession> sender, SkillCastRequest request)
        {
            Log.InfoFormat("BattleManager.ProcessBattleMessage: skill :{0} caster:{1} target:{2} position:{3}", request.castInfo.skillId, request.castInfo.casterId, request.castInfo.targetId, request.castInfo.Position.String());
            Character character = sender.Session.Character;
            var battle = MapManager.Instance[character.Info.mapId].Battle;

            battle.ProcessBattleMessage(sender, request);
        }
    }
}