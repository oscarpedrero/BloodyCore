using Bloody.Core.API.v1;
using HarmonyLib;
using ProjectM;
using System;
using Unity.Collections;


namespace Bloody.Core.Patch.Server
{

    [HarmonyPatch]
    internal class DeathVBloodPatch
    {

        public static event DeathVbloodEventHandler OnDeathVBlood;

        [HarmonyPatch(typeof(VBloodSystem), nameof(VBloodSystem.OnUpdate))]
        [HarmonyPrefix]
        private static void OnUpdate_Prefix(VBloodSystem __instance)
        {
            Core.Logger.LogDebug($"VBloodSystem.OnUpdate");
            try
            {
                OnDeathVBlood?.Invoke(__instance, __instance.EventList);
            }
            catch (Exception e)
            {
                Core.Logger.LogError(e);
            }
        }
    }
}
