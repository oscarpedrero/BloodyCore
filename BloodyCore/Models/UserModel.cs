using ProjectM.Network;
using ProjectM.Shared;
using Unity.Entities;
using Unity.Mathematics;
using Bloody.Core.Models.Base;

namespace Bloody.Core.Models
{
    public partial class UserModel : EntityModel
    {
        internal UserModel(Entity entity) : base(entity)
        {
            Character = new CharacterModel(Internals.User.Value.LocalCharacter._Entity);
            Inventory = new InventoryModel(Character);
        }

        public InventoryModel Inventory { get; }
        public CharacterModel Character { get; }

        public string CharacterName => Internals.User.Value.CharacterName.ToString();
        public EquipmentModel Equipment => Character.Equipment;
        public FromCharacter FromCharacter => new() { User = Entity, Character = Character.Entity };
        public bool IsAdmin => Internals.User.Value.IsAdmin;
        public bool IsConnected => Internals.User.Value.IsConnected;
        // 637960784686678697 not sure what is this
        //public DateTime LastConnectedUtc => Internals.User.TimeLastConnected.ToDateTime();
        public ulong PlatformId => Internals.User.Value.PlatformId;
        public float3 Position => Internals.LocalToWorld?.Position ?? new float3();
        public UserContentFlags UserContent => Internals.User.Value.UserContent;
    }
}