using ProjectM;
using Unity.Entities;
using Bloody.Core.Models.v1.Internals;
using Bloody.Core.Models.v1.Base;
using Bloody.Core.Utils.v1;

namespace Bloody.Core.Models.v1
{
    public partial class ItemModel : EntityModel
    {
        internal ItemModel(Entity entity) : base(entity)
        {
            PrefabName = Internals.PrefabGUID?.GetPrefabName();
            ManagedCore = new BaseManagedDataModel(Core.World, Internals);
        }

        public BaseManagedDataModel ManagedCore { get; }

        public string PrefabName { get; }
        public string Name => ManagedCore?.ManagedItemData?.Name.ToString();

        public ItemCategory ItemCategory => Internals.ItemData?.ItemCategory ?? ItemCategory.NONE;
        public ItemType ItemType => Internals.ItemData?.ItemType ?? 0;
        public EquipmentType EquipmentType => Internals.EquippableData?.EquipmentType ?? EquipmentType.None;
    }
}