using Bloody.Core.API.v1;
using HarmonyLib;
using ProjectM;
using System;
using Unity.Collections;
using Stunlock.Core;
using Unity.Entities;
using Bloody.Core.Models.v1;

namespace Bloody.Core.Patch.Server
{
    [HarmonyPatch]
    internal static class BuffPatch
    {
        internal static event PlayerBuffedEventHandler OnPlayerBuffed;
        internal static event PlayerBuffRemovedEventHandler OnPlayerBuffRemoved;

        [HarmonyPatch(typeof(BuffDebugSystem), nameof(BuffDebugSystem.OnUpdate))]
        [HarmonyPostfix]
        private static void BuffPatch_Postfix(BuffDebugSystem __instance)
        {
            
            //Core.Logger.LogError($"BuffDebugSystem.OnUpdate");
            try
            {
                NativeArray<Entity> entities = __instance.__query_401358786_0.ToEntityArray(Allocator.Temp);
                foreach (var buffEntity in entities)
                {
                    var prefabGuid = buffEntity.Read<PrefabGUID>();
                    var ownerEntity = buffEntity.Read<EntityOwner>().Owner;

                    if (ownerEntity.Has<PlayerCharacter>())
                    {
                        try
                        {
                            UserModel player = GameData.v1.GameData.Users.FromEntity(ownerEntity.Read<PlayerCharacter>().UserEntity);
                            OnPlayerBuffed?.Invoke(player, buffEntity, prefabGuid);
                        }
                        catch (Exception e) 
                        {
                            Core.Logger.LogError($"BuffDebugSystem.OnUpdate Error {e.Message}");
                        }
                    }
                    
                }
                entities.Dispose();

            }
            catch (Exception e)
            {
                Core.Logger.LogError(e);
            }
        }

        [HarmonyPatch(typeof(UpdateBuffsBuffer_Destroy), nameof(UpdateBuffsBuffer_Destroy.OnUpdate))]
        [HarmonyPostfix]
        public static void Prefix(UpdateBuffsBuffer_Destroy __instance)
        {
            //Core.Logger.LogError($"UpdateBuffsBuffer_Destroy.OnUpdate");
            try
            {
                var entities = __instance.__query_401358720_0.ToEntityArray(Allocator.Temp);
                foreach (var buffEntity in entities)
                {
                    var prefabGuid = buffEntity.Read<PrefabGUID>();
                    var ownerEntity = buffEntity.Read<EntityOwner>().Owner;

                    if (ownerEntity.Has<PlayerCharacter>())
                    {
                        try
                        {
                            UserModel player = GameData.v1.GameData.Users.FromEntity(ownerEntity.Read<PlayerCharacter>().UserEntity);
                            OnPlayerBuffRemoved?.Invoke(player, buffEntity, prefabGuid);
                        }
                        catch
                        {

                        }
                    }


                }
                entities.Dispose();
            }
            catch (Exception e)
            {
                Core.Logger.LogError(e);
            }
        }
    }
}
