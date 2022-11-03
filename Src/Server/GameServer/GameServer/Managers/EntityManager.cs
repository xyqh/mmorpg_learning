using Common;
using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class EntityManager : Singleton<EntityManager>
    {
        private int idx = 0;
        //public List<Entity> AllEntities = new List<Entity>();
        public Dictionary<int, List<Entity>> MapEntities = new Dictionary<int, List<Entity>>();
        public Dictionary<int, Entity> AllEntities = new Dictionary<int, Entity>();

        public void AddEntity(int mapId, Entity entity)
        {
            // 加入管理器生成唯一id
            entity.EntityData.Id = ++idx;
            AllEntities.Add(entity.EntityData.Id, entity);

            List<Entity> entities = null;
            if(!MapEntities.TryGetValue(mapId, out entities))
            {
                entities = new List<Entity>();
                MapEntities[mapId] = entities;
            }

            entities.Add(entity);
            Log.InfoFormat("AddEntity:{0}", entity.entityId);
        }

        public void RemoveEntity(int mapId, Entity entity)
        {
            AllEntities.Remove(entity.entityId);
            if (MapEntities.ContainsKey(mapId))
            {
                MapEntities[mapId].Remove(entity);
            }
        }

        public Entity GetEntity(int entityId)
        {
            Entity entity = null;
            AllEntities.TryGetValue(entityId, out entity);
            return entity;
        }

        internal Creature GetCreature(int entityId)
        {
            return this.GetEntity(entityId) as Creature;
        }
    }
}
