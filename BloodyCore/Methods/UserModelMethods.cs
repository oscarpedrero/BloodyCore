using System;
using System.Runtime.InteropServices;
using Bloodstone.API;
using ProjectM;
using ProjectM.CastleBuilding;
using ProjectM.Network;
using Stunlock.Core;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Bloody.Core.API.v1;
using Bloody.Core.Models.v1;
using Bloody.Core.Utils.v1;

namespace Bloody.Core.Methods
{
    public static class UserModelMethods
    {
        public static void SendSystemMessage(this UserModel userModel, string message)
        {
            if (Core.World.IsClientWorld())
            {
                return;
            }
            ServerChatUtils.SendSystemMessageToClient(Core.World.EntityManager, userModel.Internals.User.Value, message);
        }

        public static bool TryGiveItem(this UserModel userModel, PrefabGUID itemGuid, int amount, out Entity itemEntity)
        {
            itemEntity = Entity.Null;
            if (Core.World.IsClientWorld())
            {
                return false;
            }

            unsafe
            {
                /*var bytes = stackalloc byte[Marshal.SizeOf<FakeNull>()];
                var bytePtr = new IntPtr(bytes);
                Marshal.StructureToPtr(new FakeNull { value = 0, has_value = true }, bytePtr, false);
                var boxedBytePtr = IntPtr.Subtract(bytePtr, 0x10);
                var hack = new Il2CppSystem.Nullable<int>(boxedBytePtr);
                var GameData = Core.SystemsCore.GameDataSystem;
                var itemSettings = AddItemSettings.Create(VWorld.Server.EntityManager, GameData.ItemHashLookupMap);
                // public unsafe static AddItemResponse TryAddItem([DefaultParameterValue(null)] AddItemSettings addItemSettings, [DefaultParameterValue(null)] Entity target, [DefaultParameterValue(null)] InventoryBuffer inventoryItem)
                if (InventoryUtilitiesServer.TryAddItem(itemSettings, userModel.Internals.User.Value.LocalCharacter._Entity, itemGuid, amount))
                {
                    itemEntity = itemSettings.PreviousItemEntity;
                    return true;
                }

                return false;*/
                if(UserSystem.TryAddUserInventoryItem(userModel.Character.Entity, itemGuid, amount).Success)
                {
                    return true;
                }
                return false;
            }
        }

        public static void DropItemNearby(this UserModel userModel, PrefabGUID itemGuid, int amount)
        {
            if (Core.World.IsClientWorld())
            {
                return;
            }
            InventoryUtilitiesServer.CreateDropItem(Core.World.EntityManager, userModel.Entity, itemGuid, amount, new Entity());
        }

        public static void TeleportTo(this UserModel userModel, float3 position)
        {
            if (Core.World.IsClientWorld())
            {
                return;
            }
            var entity = Core.World.EntityManager.CreateEntity(
                ComponentType.ReadWrite<FromCharacter>(),
                ComponentType.ReadWrite<PlayerTeleportDebugEvent>()
            );

            Core.World.EntityManager.SetComponentData(entity, userModel.FromCharacter);

            Core.World.EntityManager.SetComponentData<PlayerTeleportDebugEvent>(
                entity,
                new() { Position = position, Target = PlayerTeleportDebugEvent.TeleportTarget.Self });
        }

        public static bool IsInCastle(this UserModel userModel)
        {
            var query = Core.World.EntityManager.CreateEntityQuery(
                ComponentType.ReadOnly<PrefabGUID>(),
                ComponentType.ReadOnly<LocalToWorld>(),
                ComponentType.ReadOnly<UserOwner>(),
                ComponentType.ReadOnly<CastleFloor>());

            foreach (var entityModel in query.ToEnumerable())
            {
                if (entityModel.LocalToWorld == null)
                {
                    continue;
                }
                var localToWorld = entityModel.LocalToWorld.Value;
                var position = localToWorld.Position;
                var userPosition = userModel.Position;
                if (Math.Abs(userPosition.x - position.x) < 3 && Math.Abs(userPosition.z - position.z) < 3)
                {
                    return true;
                }
            }
            return false;
        }

        private static readonly PrefabGUID InCombatBuff = new PrefabGUID(581443919);
        private static readonly PrefabGUID InCombatPvPBuff = new PrefabGUID(697095869);
        public static bool IsInCombat(this UserModel userModel)
        {
            return BuffUtility.HasBuff(Core.World.EntityManager, userModel.Character.Entity, InCombatBuff) ||
                   BuffUtility.HasBuff(Core.World.EntityManager, userModel.Character.Entity, InCombatPvPBuff);

        }
    }
}