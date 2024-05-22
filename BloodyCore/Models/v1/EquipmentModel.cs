using ProjectM;
using Unity.Entities;

namespace Bloody.Core.Models.v1
{
    public partial class EquipmentModel
    {
        public Equipment Internals { get; }

        public ItemModel Chest => Internals.ArmorChestSlot.SlotEntity._Entity != Entity.Null
            ? new ItemModel(Internals.ArmorChestSlot.SlotEntity._Entity)
            : null;

        public ItemModel Leg => Internals.ArmorLegsSlot.SlotEntity._Entity != Entity.Null
            ? new ItemModel(Internals.ArmorLegsSlot.SlotEntity._Entity)
            : null;

        public ItemModel Headgear => Internals.ArmorHeadgearSlot.SlotEntity._Entity != Entity.Null
            ? new ItemModel(Internals.ArmorHeadgearSlot.SlotEntity._Entity)
            : null;

        public ItemModel Footgear => Internals.ArmorFootgearSlot.SlotEntity._Entity != Entity.Null
            ? new ItemModel(Internals.ArmorFootgearSlot.SlotEntity._Entity)
            : null;

        public ItemModel Gloves => Internals.ArmorGlovesSlot.SlotEntity._Entity != Entity.Null
            ? new ItemModel(Internals.ArmorGlovesSlot.SlotEntity._Entity)
            : null;

        public ItemModel Cloak => Internals.CloakSlot.SlotEntity._Entity != Entity.Null
            ? new ItemModel(Internals.CloakSlot.SlotEntity._Entity)
            : null;

        public ItemModel Weapon => Internals.WeaponSlot.SlotEntity._Entity != Entity.Null
            ? new ItemModel(Internals.WeaponSlot.SlotEntity._Entity)
            : null;

        public ItemModel Jewelry => Internals.GrimoireSlot.SlotEntity._Entity != Entity.Null
            ? new ItemModel(Internals.GrimoireSlot.SlotEntity._Entity)
            : null;

        public float ArmorLevel => Internals.ArmorLevel.Value;
        public float WeaponLevel => Internals.WeaponLevel.Value;
        public float SpellLevel => Internals.SpellLevel.Value;

        public float Level => ArmorLevel + WeaponLevel + SpellLevel;

        public EquipmentModel(Equipment equipment)
        {
            Internals = equipment;
        }
    }
}