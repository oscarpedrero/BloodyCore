using HarmonyLib;
using ProjectM;
using Unity.Entities;
using Unity.Collections;
using System;
using Bloody.Core.API.v1;

namespace Bloody.Core.Patch.v1.Server
{

    [HarmonyPatch]
    internal class TraderPurchasePatch
    {

        internal static event TraderPurchaseEventHandler OnTraderPurchase;

        [HarmonyPatch(typeof(TraderPurchaseSystem), nameof(TraderPurchaseSystem.OnUpdate))]
        [HarmonyPrefix]
        internal static void Prefix(TraderPurchaseSystem __instance)
        {
            try
            {
                var _entities = __instance._TraderPurchaseEventQuery.ToEntityArray(Allocator.Temp);
                OnTraderPurchase?.Invoke(_entities);
            }
            catch (Exception e)
            {
                Core.Logger.LogError(e);
            }

        }
    }
}
