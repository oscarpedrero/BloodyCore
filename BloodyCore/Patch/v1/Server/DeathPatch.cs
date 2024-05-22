using Bloody.Core.API.v1;
using HarmonyLib;
using ProjectM;
using System;
using Unity.Collections;

namespace Bloody.Core.Patch.v1.Server
{


    [HarmonyPatch]
    internal class DeathPatch
    {
        internal static event DeathEventHandler OnDeath;

        [HarmonyPatch(typeof(DeathEventListenerSystem), nameof(DeathEventListenerSystem.OnUpdate))]
        [HarmonyPostfix]
        private static void DeathEventListenerSystemPatch_Postfix(DeathEventListenerSystem __instance)
        {

            Core.Logger.LogDebug($"DeathEventListenerSystem.OnUpdate");
            try
            {
                var deathEvents = __instance._DeathEventQuery.ToComponentDataArray<DeathEvent>(Allocator.Temp);

                OnDeath?.Invoke(__instance, deathEvents);
            }
            catch (Exception e)
            {
                Core.Logger.LogError(e);
            }
        }
    }
}
