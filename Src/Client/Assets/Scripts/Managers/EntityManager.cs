using Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    interface IEntityNotify
    {
        void OnEntityRemove();
        void OnEntityChanged(Entity entity);
        void OnEntityEvent(EntityEvent @event);
    }

    class EntityManager:Singleton<EntityManager>
    {
        Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        Dictionary<int, IEntityNotify> notifiers = new Dictionary<int, IEntityNotify>();

        public void RegisterEntityChangeNotify(int entityId, IEntityNotify notify)
        {
            notifiers[entityId] = notify;
        }

        public void AddEntity(Entity entity)
        {
            entities[entity.entityId] = entity;
        }

        public void RemoveEntity(NEntity entity)
        {
            int id = entity.Id;
            entities.Remove(id);
            if (notifiers.ContainsKey(id))
            {
                notifiers[id].OnEntityRemove();
                notifiers.Remove(id);
            }
        }

        internal void OnEntitySync(NEntitySync data)
        {
            Entity entity = null;
            entities.TryGetValue(data.Id, out entity);
            if(entity != null)
            {
                if(data.Entity != null)
                {
                    entity.EntityData = data.Entity;
                }
                if (notifiers.ContainsKey(data.Id))
                {
                    notifiers[entity.entityId].OnEntityChanged(entity);
                    notifiers[entity.entityId].OnEntityEvent(data.Event);
                }
            }
        }

        public Entity GetEntity(int casterId)
        {
            Entity entity = null;
            entities.TryGetValue(casterId, out entity);
            return entity;
        }
    }
}
