using HarmonyLib;
using ProjectM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodyCore.Patch.Client
{
    internal static class OnGameClientDataInitializedPatch
    {
        internal static event OnGameDataInitializedEventHandler OnGameDataInitialized;
        internal static event OnGameDataDestroyedEventHandler OnGameDataDestroyed;

        private static bool _onGameDataInitializedTriggered;
        [HarmonyPatch(typeof(GameDataManager), "OnUpdate")]
        [HarmonyPostfix]
        private static void GameDataManagerOnUpdatePostfix(GameDataManager __instance)
        {
            if (_onGameDataInitializedTriggered)
            {
                return;
            }

            try
            {
                if (!__instance.GameDataInitialized)
                {
                    return;
                }

                _onGameDataInitializedTriggered = true;
                BloodyCore.Logger.LogDebug("GameDataManagerOnUpdatePostfix Trigger");
                OnGameDataInitialized?.Invoke(__instance.World);
            }
            catch (Exception ex)
            {
                BloodyCore.Logger.LogError(ex);
            }
        }

        [HarmonyPatch(typeof(ClientBootstrapSystem), "OnDestroy")]
        [HarmonyPostfix]
        private static void ClientBootstrapSystemOnDestroyPostfix(ClientBootstrapSystem __instance)
        {
            _onGameDataInitializedTriggered = false;
            try
            {
                OnGameDataDestroyed?.Invoke();
            }
            catch (Exception ex)
            {
                BloodyCore.Logger.LogError(ex);
            }
        }
    }
}
