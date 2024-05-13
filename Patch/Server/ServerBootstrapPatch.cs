using HarmonyLib;
using ProjectM;
using Stunlock.Network;
using System;

namespace BloodyCore.Patch.Server
{
    public delegate void OnUserConnectedEventHandler(ServerBootstrapSystem sender, NetConnectionId netConnectionId);
    public delegate void OnUserDisconnectedEventHandler(ServerBootstrapSystem sender, NetConnectionId netConnectionId, ConnectionStatusChangeReason connectionStatusReason, string extraData);

    public class ServerBootstrapPatch
    {

        public static event OnUserConnectedEventHandler OnUserConnected;
        public static event OnUserDisconnectedEventHandler OnUserDisconnected;

        [HarmonyPatch(typeof(ServerBootstrapSystem), nameof(ServerBootstrapSystem.OnUserConnected))]
        [HarmonyPrefix]
        private static void Postfix(ServerBootstrapSystem __instance, NetConnectionId netConnectionId)
        {
            BloodyCore.Logger.LogDebug($"ServerBootstrapSystem.OnUserConnected");
            try
            {

                OnUserConnected?.Invoke(__instance, netConnectionId);
            }
            catch (Exception e)
            {
                BloodyCore.Logger.LogError(e);
            }
        }

        [HarmonyPatch(typeof(ServerBootstrapSystem), nameof(ServerBootstrapSystem.OnUserDisconnected))]
        [HarmonyPrefix]
        private static void Prefix(ServerBootstrapSystem __instance, NetConnectionId netConnectionId, ConnectionStatusChangeReason connectionStatusReason, string extraData)
        {

            BloodyCore.Logger.LogDebug($"ServerBootstrapSystem.OnUserDisconnected");
            try
            {
                OnUserDisconnected?.Invoke(__instance, netConnectionId, connectionStatusReason, extraData);
            }
            catch (Exception e)
            {
                BloodyCore.Logger.LogError(e);
            }

        }
    }
}
