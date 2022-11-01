using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using GameServer.Entities;

namespace GameServer.Managers
{
    class CharacterManager : Singleton<CharacterManager>
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();

        public CharacterManager()
        {
        }

        public void Dispose()
        {
        }

        public void Init()
        {

        }

        public void Clear()
        {
            this.Characters.Clear();
        }

        public Character AddCharacter(TCharacter cha)
        {
            Character character = new Character(CharacterType.Player, cha);
            EntityManager.Instance.AddEntity(cha.MapID, character);
            Log.InfoFormat("AddCharacter:{0}, {1}, {2}", cha.Name, cha.Player.ToString(), character.entityId);
            character.Info.EntityId = character.entityId;
            this.Characters[cha.ID] = character;
            return character;
        }


        public void RemoveCharacter(int characterId)
        {
            Character cha = null;
            Characters.TryGetValue(characterId, out cha);
            if (cha == null) return;
            EntityManager.Instance.RemoveEntity(cha.Data.MapID, cha);
            this.Characters.Remove(characterId);
        }

        public Character GetCharacter(int characterId)
        {
            Character cha = null;
            Characters.TryGetValue(characterId, out cha);
            return cha;
        }
    }
}
