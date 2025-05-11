using System;
using HarmonyLib;
using ProjectM;
using Unity.Entities;
using Bloody.Core.ClassGenerator.Utils;

namespace Bloody.Core.ClassGenerator.Patch
{
    public delegate void ServerStartupStateChangeEventHandler(LoadPersistenceSystemV2 sender, ServerStartupState.State serverStartupState);
    public delegate void OnGameDataInitializedEventHandler(World world);

    public static class ServerEvents
    {
        public static event ServerStartupStateChangeEventHandler OnServerStartupStateChanged;

        internal static event OnGameDataInitializedEventHandler OnCoreInitialized;

        [HarmonyPatch(typeof(LoadPersistenceSystemV2), nameof(LoadPersistenceSystemV2.SetLoadState))]
        [HarmonyPostfix]
        private static void ServerStartupStateChange_Postfix(ServerStartupState.State loadState, LoadPersistenceSystemV2 __instance)
        {
            try
            {
                Plugin.Logger.LogDebug("ServerStartupStateChange_Postfix");
                Plugin.Logger.LogDebug(ServerStartupState.State.SuccessfulStartup.ToString());
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
                Plugin.Logger.LogError(ex);
            }
        }

        [HarmonyPatch(typeof(LoadPersistenceSystemV2), nameof(LoadPersistenceSystemV2.SetLoadState))]
        private static void ServerStartupStateChange_Prefix(ServerStartupState.State loadState, LoadPersistenceSystemV2 __instance)
        {
            Plugin.Logger.LogMessage($"OnCreateConsoleCommands");
            Plugin.Logger.LogMessage($"{loadState}");
            try
            {
                OnServerStartupStateChanged?.Invoke(__instance, loadState);
            }
            catch (Exception e)
            {
                Plugin.Logger.LogError(e);
            }
        }

        [HarmonyPatch(typeof(ClientConsoleCommandSystem), "OnCreateConsoleCommands")]
        [HarmonyPrefix]
        private static void Prefix(ClientConsoleCommandSystem __instance)
        {
            Plugin.Logger.LogMessage($"OnCreateConsoleCommands");
            try
            {
                new CreateGameDataClassesConsoleCommand().Register(__instance);
            }
            catch (Exception e)
            {
                Plugin.Logger.LogError(e);
            }
        }
    }
}


