using Bloody.Core.API;
using HarmonyLib;
using ProjectM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloody.Core.Patch.Client
{
    internal static class OnGameClientDataInitializedPatch
    {
        internal static event OnGameDataInitializedEventHandler OnCoreInitialized;
        internal static event OnGameDataDestroyedEventHandler OnCoreDestroyed;

        private static bool _onCoreInitializedTriggered;
        [HarmonyPatch(typeof(GameDataManager), "OnUpdate")]
        [HarmonyPostfix]
        private static void CoreManagerOnUpdatePostfix(GameDataManager __instance)
        {
            if (_onCoreInitializedTriggered)
            {
                return;
            }

            try
            {
                if (!__instance.GameDataInitialized)
                {
                    return;
                }

                _onCoreInitializedTriggered = true;
                Core.Logger.LogDebug("CoreManagerOnUpdatePostfix Trigger");
                OnCoreInitialized?.Invoke(__instance.World);
            }
            catch (Exception ex)
            {
                Core.Logger.LogError(ex);
            }
        }

        [HarmonyPatch(typeof(ClientBootstrapSystem), "OnDestroy")]
        [HarmonyPostfix]
        private static void ClientBootstrapSystemOnDestroyPostfix(ClientBootstrapSystem __instance)
        {
            _onCoreInitializedTriggered = false;
            try
            {
                OnCoreDestroyed?.Invoke();
            }
            catch (Exception ex)
            {
                Core.Logger.LogError(ex);
            }
        }
    }
}
