using Bloody.Core.API.v1;
using HarmonyLib;
using ProjectM;
using ProjectM.Gameplay.Systems;
using System;
using Unity.Collections;

namespace Bloody.Core.Patch.Server
{


    [HarmonyPatch]
    internal class DealDamageSystemPatch
    {
        internal static event DamageEventHandler OnDamage;

        [HarmonyPatch(typeof(DealDamageSystem), nameof(DealDamageSystem.DealDamage))]
        [HarmonyPostfix]
        private static void DealDamageSystemPatch_Postfix(DealDamageSystem __instance)
        {

            //Core.Logger.LogDebug($"DealDamageSystem.OnUpdate");
            try
            {
                var damageTakenEvent = __instance._Query.ToComponentDataArray<DealDamageEvent>(Allocator.Temp);

                OnDamage?.Invoke(__instance, damageTakenEvent);
            }
            catch (Exception e)
            {
                Core.Logger.LogError(e);
            }
        }
    }
}
