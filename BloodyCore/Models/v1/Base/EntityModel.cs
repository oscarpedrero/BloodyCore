using ProjectM;
using Stunlock.Core;
using Unity.Entities;
using Bloody.Core.Models.v1.Internals;

namespace Bloody.Core.Models.v1.Base
{
    public abstract class EntityModel
    {
        protected EntityModel(Entity entity)
        {
            Entity = entity;
            Internals = new BaseEntityModel(Core.World, entity);
        }

        public Entity Entity { get; }

        public PrefabGUID PrefabGUID => Internals.PrefabGUID ?? new PrefabGUID();

        public BaseEntityModel Internals { get; }
    }
}