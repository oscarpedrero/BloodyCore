using HarmonyLib;
using ProjectM;
using System;
using Unity.Collections;

namespace BloodyCore.Patch.Server
{
    public delegate void DeathEventHandler(DeathEventListenerSystem sender, NativeArray<DeathEvent> deathEvents);

    [HarmonyPatch]
    internal class DeathPatch
    {
        public static event DeathEventHandler OnDeath;

        [HarmonyPatch(typeof(DeathEventListenerSystem), nameof(DeathEventListenerSystem.OnUpdate))]
        [HarmonyPostfix]
        private static void DeathEventListenerSystemPatch_Postfix(DeathEventListenerSystem __instance)
        {

            BloodyCore.Logger.LogDebug($"DeathEventListenerSystem.OnUpdate");
            try
            {
                var deathEvents = __instance._DeathEventQuery.ToComponentDataArray<DeathEvent>(Allocator.Temp);

                if (deathEvents.Length > 0)
                {
                    OnDeath?.Invoke(__instance, deathEvents);
                }
            }
            catch (Exception e)
            {
                BloodyCore.Logger.LogError(e);
            }
        }
    }
}
