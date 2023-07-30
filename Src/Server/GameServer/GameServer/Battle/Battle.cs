﻿using GameServer.Core;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameServer.Battle
{
    class Battle
    {
        public Map Map;

        Dictionary<int, Creature> AllUnits = new Dictionary<int, Creature>();

        Queue<NSkillCastInfo> Actions = new Queue<NSkillCastInfo>();

        List<Creature> DeahPool = new List<Creature>();

        List<NSkillHitInfo> Hits = new List<NSkillHitInfo>();

        public Battle(Map map)
        {
            this.Map = map;
        }

        internal void ProcessBattleMessage(NetConnection<NetSession> sender, SkillCastRequest request)
        {
            Character character = sender.Session.Character;
            if(request.castInfo != null)
            {
                if(character.entityId != request.castInfo.casterId)
                {
                    return;
                }

                this.Actions.Enqueue(request.castInfo);
            }
        }

        internal void Update()
        {
            this.Hits.Clear();
            if(this.Actions.Count > 0)
            {
                NSkillCastInfo skillCast = this.Actions.Dequeue();
                this.ExecuteAction(skillCast);
            }

            this.UpdateUnits();
            this.BroadcastHitsMessage();
        }

        private void BroadcastHitsMessage()
        {
            if (this.Hits.Count == 0) return;
            NetMessageResponse message = new NetMessageResponse();
            message.skillHits = new SkillHitResponse();
            message.skillHits.Hits.AddRange(this.Hits);
            message.skillHits.Result = Result.Success;
            message.skillHits.Errormsg = "";
            this.Map.BroadcastBattleResponse(message);
        }

        public void JoinBattle(Creature unit)
        {
            this.AllUnits[unit.entityId] = unit;
        }

        public void LeaveBattle(Creature unit)
        {
            this.AllUnits.Remove(unit.entityId);
        }

        void ExecuteAction(NSkillCastInfo cast)
        {
            BattleContext context = new BattleContext(this);
            context.Caster = EntityManager.Instance.GetCreature(cast.casterId);
            context.Target = EntityManager.Instance.GetCreature(cast.targetId);
            context.CastSkill = cast;
            if(context.Caster != null)
            {
                this.JoinBattle(context.Caster);
            }
            if(context.Target != null)
            {
                this.JoinBattle(context.Target);
            }
            context.Caster.CastSkill(context, cast.skillId);

            NetMessageResponse message = new NetMessageResponse();
            message.skillCast = new SkillCastResponse();
            message.skillCast.castInfo = context.CastSkill;
            message.skillCast.Damage = context.Damage;
            message.skillCast.Result = context.Result == SkillResult.Ok ? Result.Success : Result.Failed;
            message.skillCast.Errormsg = context.Result.ToString();
            this.Map.BroadcastBattleResponse(message);
        }

        void UpdateUnits()
        {
            this.DeahPool.Clear();
            foreach(var kv in this.AllUnits)
            {
                kv.Value.Update();
                if (kv.Value.IsDeath)
                {
                    this.DeahPool.Add(kv.Value);
                }
            }

            foreach(var unit in this.DeahPool)
            {
                this.LeaveBattle(unit);
            }
        }

        public List<Creature> FindUnitsInRange(Vector3Int pos, int range)
        {
            List<Creature> result = new List<Creature>();
            foreach(var unit in this.AllUnits)
            {
                if(unit.Value.Distance(pos) < range)
                {
                    result.Add(unit.Value);
                }
            }
            return result;
        }

        public void AddHitInfo(NSkillHitInfo hit)
        {
            this.Hits.Add(hit);
        }
    }
}