using Bloodstone.API;
using Bloody.Core.API.v1;
using HarmonyLib;
using ProjectM;
using System;

namespace Bloody.Core.Patch.Server
{
    internal static class OnGameServerDataInitializedPatch
    {
        internal static event OnGameDataInitializedEventHandler OnCoreInitialized;

        [HarmonyPatch(typeof(LoadPersistenceSystemV2), nameof(LoadPersistenceSystemV2.SetLoadState))]
        [HarmonyPostfix]
        private static void ServerStartupStateChange_Postfix(ServerStartupState.State loadState, LoadPersistenceSystemV2 __instance)
        {
            try
            {
                //Core.Logger.LogDebug("ServerStartupStateChange_Postfix");
                //Core.Logger.LogDebug(ServerStartupState.State.SuccessfulStartup.ToString());
                if (loadState == ServerStartupState.State.SuccessfulStartup)
                {
                    var world = VWorld.Server;

                    if (VWorld.IsServer)
                    {
                        world = VWorld.Server;
                    }
                    else
                    {
                        world = VWorld.Client;
                    }
                    OnCoreInitialized?.Invoke(world);
                }
            }
            catch (Exception ex)
            {
                Core.Logger.LogError(ex);
            }
        }
    }
}
