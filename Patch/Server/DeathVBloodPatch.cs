using HarmonyLib;
using ProjectM;
using System;
using Unity.Collections;


namespace BloodyCore.Patch.Server
{
    public delegate void DeathVbloodEventHandler(VBloodSystem sender, NativeArray<VBloodConsumed> deathEvents);

    [HarmonyPatch]
    internal class DeathVBloodPatch
    {

        public static event DeathVbloodEventHandler OnDeathVBlood;

        [HarmonyPatch(typeof(VBloodSystem), nameof(VBloodSystem.OnUpdate))]
        [HarmonyPrefix]
        public static void OnUpdate_Prefix(VBloodSystem __instance)
        {
            if (__instance.EventList.Length > 0)
            {
                BloodyCore.Logger.LogDebug($"VBloodSystem.OnUpdate");
                try
                {
                    var deathVBloodEvents = __instance._Query.ToComponentDataArray<VBloodConsumed>(Allocator.Temp);

                    if (deathVBloodEvents.Length > 0)
                    {
                        OnDeathVBlood?.Invoke(__instance, deathVBloodEvents);
                    }
                }
                catch (Exception e)
                {
                    BloodyCore.Logger.LogError(e);
                }
            }
        }
    }
}
