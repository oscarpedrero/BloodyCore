using Unity.Entities;
using Bloody.Core.Models.v1.Internals;

namespace Bloody.Core.Models.v1
{
    public partial class CharacterModel
    {
        public Entity Entity { get; }
        public BaseEntityModel Internals { get; }

        public EquipmentModel Equipment => Internals.Equipment != null ? new(Internals.Equipment.Value) : null;

        internal CharacterModel(Entity entity)
        {
            Entity = entity;
            Internals = new BaseEntityModel(Core.World, entity);
        }
    }
}