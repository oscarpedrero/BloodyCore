using HarmonyLib;
using ProjectM;
using Unity.Entities;
using Unity.Collections;
using System;

namespace BloodyCore.Patch.Server
{

    public delegate void TraderPurchaseEventHandler(NativeArray<Entity> entity);

    [HarmonyPatch]
    public class TraderPurchasePatch
    {

        public static event TraderPurchaseEventHandler OnTraderPurchase;

        [HarmonyPatch(typeof(TraderPurchaseSystem), nameof(TraderPurchaseSystem.OnUpdate))]
        [HarmonyPrefix]
        private static void Prefix(TraderPurchaseSystem __instance)
        {
            try
            {
                var _entities = __instance._TraderPurchaseEventQuery.ToEntityArray(Allocator.Temp);
                OnTraderPurchase?.Invoke(_entities);
            }
            catch (Exception e)
            {
                BloodyCore.Logger.LogError(e);
            }

        }
    }
}
