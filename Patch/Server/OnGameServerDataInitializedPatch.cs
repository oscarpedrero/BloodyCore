using Bloodstone.API;
using HarmonyLib;
using ProjectM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodyCore.Patch.Server
{
    internal static class OnGameServerDataInitializedPatch
    {
        internal static event OnGameDataInitializedEventHandler OnGameDataInitialized;

        [HarmonyPatch(typeof(LoadPersistenceSystemV2), nameof(LoadPersistenceSystemV2.SetLoadState))]
        [HarmonyPostfix]
        private static void Postfix(ServerStartupState.State loadState, LoadPersistenceSystemV2 __instance)
        {
            try
            {
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
                    OnGameDataInitialized?.Invoke(world);
                }
            }
            catch (Exception ex)
            {
                BloodyCore.Logger.LogError(ex);
            }
        }
    }
}
