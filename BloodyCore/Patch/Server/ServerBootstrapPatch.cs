using Bloody.Core.API;
using HarmonyLib;
using ProjectM;
using Stunlock.Network;
using System;

namespace Bloody.Core.Patch.Server
{
    

    internal class ServerBootstrapPatch
    {

        internal static event OnUserConnectedEventHandler OnUserConnected;
        internal static event OnUserDisconnectedEventHandler OnUserDisconnected;

        [HarmonyPatch(typeof(ServerBootstrapSystem), nameof(ServerBootstrapSystem.OnUserConnected))]
        [HarmonyPrefix]
        internal static void Postfix(ServerBootstrapSystem __instance, NetConnectionId netConnectionId)
        {
            Core.Logger.LogDebug($"ServerBootstrapSystem.OnUserConnected");
            try
            {

                OnUserConnected?.Invoke(__instance, netConnectionId);
            }
            catch (Exception e)
            {
                Core.Logger.LogError(e);
            }
        }

        [HarmonyPatch(typeof(ServerBootstrapSystem), nameof(ServerBootstrapSystem.OnUserDisconnected))]
        [HarmonyPrefix]
        internal static void Prefix(ServerBootstrapSystem __instance, NetConnectionId netConnectionId, ConnectionStatusChangeReason connectionStatusReason, string extraData)
        {

            Core.Logger.LogDebug($"ServerBootstrapSystem.OnUserDisconnected");
            try
            {
                OnUserDisconnected?.Invoke(__instance, netConnectionId, connectionStatusReason, extraData);
            }
            catch (Exception e)
            {
                Core.Logger.LogError(e);
            }

        }
    }
}
