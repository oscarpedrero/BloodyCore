using Bloody.Core.API.v1;
using HarmonyLib;
using ProjectM;
using Unity.Collections;

namespace Bloody.Core.Patch.Server
{
    [HarmonyPatch]
    internal class VampireDownedPatch
    {

        public static event VampireDownedHandler OnVampireDowned;

        [HarmonyPatch(typeof(VampireDownedServerEventSystem), nameof(VampireDownedServerEventSystem.OnUpdate))]
        [HarmonyPrefix]
        internal static void Prefix(VampireDownedServerEventSystem __instance)
        {
            var downedEvents = __instance.__query_1174204813_0.ToEntityArray(Allocator.Temp);

            OnVampireDowned?.Invoke(__instance, downedEvents);
      

        }
    }
}
