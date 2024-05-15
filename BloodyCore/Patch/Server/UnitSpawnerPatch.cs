using Bloodstone.API;
using Bloody.Core.API;
using HarmonyLib;
using ProjectM;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Bloody.Core.Patch.Server
{

    [HarmonyPatch]
    internal class UnitSpawnerPatch
    {
        internal static event OnUnitSpawnedEventHandler OnUnitSpawned;

        internal static Dictionary<long, (float actualDuration, Action<Entity> Actions)> PostActions = new();
        internal static bool EnabledCallBack { get; set; } = false;

        [HarmonyPatch(typeof(UnitSpawnerReactSystem), nameof(UnitSpawnerReactSystem.OnUpdate))]
        [HarmonyPostfix]
        internal static void Prefix(UnitSpawnerReactSystem __instance)
        {

            Core.Logger.LogDebug($"UnitSpawnerReactSystem.OnUpdate");
            try
            {
                var _entities = __instance._Query.ToEntityArray(Allocator.Temp);

                if (!EnabledCallBack) OnUnitSpawned?.Invoke(_entities);

                foreach (var entity in _entities)
                {

                    if (!EnabledCallBack) return;

                    if (!entity.Has<LifeTime>()) return;

                    var lifetimeComp = entity.Read<LifeTime>();
                    var durationKey = (long)Mathf.Round(lifetimeComp.Duration);
                    Core.Logger.LogDebug($"Found durationKey {durationKey} from LifeTime({lifetimeComp.Duration})");
                    if (PostActions.TryGetValue(durationKey, out var unitData))
                    {
                        var (actualDuration, actions) = unitData;
                        Core.Logger.LogDebug($"Found post actions for {durationKey} with {actualDuration} duration");
                        PostActions.Remove(durationKey);

                        var endAction = actualDuration < 0 ? LifeTimeEndAction.None : LifeTimeEndAction.Destroy;

                        var newLifeTime = new LifeTime()
                        {
                            Duration = actualDuration,
                            EndAction = endAction
                        };

                        entity.Write(newLifeTime);
                        actions(entity);
                    }
                }

                EnabledCallBack = false;
            }
            catch (Exception e)
            {
                Core.Logger.LogError(e);
            }


        }
    }
}
