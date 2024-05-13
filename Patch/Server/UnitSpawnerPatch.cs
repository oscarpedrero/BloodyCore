using Bloodstone.API;
using HarmonyLib;
using ProjectM;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace BloodyCore.Patch.Server
{

    public delegate void OnUnitSpawnedEventHandler(NativeArray<Entity> entity);

    [HarmonyPatch]
    public class UnitSpawnerPatch
    {
        public static event OnUnitSpawnedEventHandler OnUnitSpawned;

        internal static Dictionary<long, (float actualDuration, Action<Entity> Actions)> PostActions = new();
        public static bool EnabledCallBack { get; set; } = false;

        [HarmonyPatch(typeof(UnitSpawnerReactSystem), nameof(UnitSpawnerReactSystem.OnUpdate))]
        [HarmonyPostfix]
        internal static void Prefix(UnitSpawnerReactSystem __instance)
        {

            BloodyCore.Logger.LogDebug($"UnitSpawnerReactSystem.OnUpdate");
            try
            {
                var _entities = __instance._Query.ToEntityArray(Allocator.Temp);

                if (!EnabledCallBack) OnUnitSpawned?.Invoke(_entities);

                foreach (var entity in _entities)
                {

                        if (!EnabledCallBack) return;

                        if (!BloodyCore.Systems.EntityManager.HasComponent<LifeTime>(entity)) return;

                        var lifetimeComp = BloodyCore.Systems.EntityManager.GetComponentData<LifeTime>(entity);
                        var durationKey = (long)Mathf.Round(lifetimeComp.Duration);
                        BloodyCore.Logger.LogDebug($"Found durationKey {durationKey} from LifeTime({lifetimeComp.Duration})");
                        if (PostActions.TryGetValue(durationKey, out var unitData))
                        {
                            var (actualDuration, actions) = unitData;
                            BloodyCore.Logger.LogDebug($"Found post actions for {durationKey} with {actualDuration} duration");
                            PostActions.Remove(durationKey);

                            var endAction = actualDuration < 0 ? LifeTimeEndAction.None : LifeTimeEndAction.Destroy;

                            var newLifeTime = new LifeTime()
                            {
                                Duration = actualDuration,
                                EndAction = endAction
                            };

                            BloodyCore.Systems.EntityManager.SetComponentData(entity, newLifeTime);

                            actions(entity);
                        }
                }
            }
            catch (Exception e)
            {
                BloodyCore.Logger.LogError(e);
            }


        }
    }
}
