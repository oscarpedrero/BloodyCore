using Bloody.Core.API.v1;
using HarmonyLib;
using ProjectM;
using Unity.Collections;

namespace Bloody.Core.Patch.v1.Server
{
    [HarmonyPatch]
    internal class VampireDownedPatch
    {

        public static event VampireDownedHandler OnVampireDowned;

        [HarmonyPatch(typeof(VampireDownedServerEventSystem), nameof(VampireDownedServerEventSystem.OnUpdate))]
        [HarmonyPostfix]
        internal static void VampireDownedServerEventSystem_Postfix(VampireDownedServerEventSystem __instance)
        {

            var vampireDownedEntitys = __instance.__query_1174204813_0.ToEntityArray(Allocator.Temp);
            if (vampireDownedEntitys.Length > 0)
            {
                OnVampireDowned?.Invoke(__instance, vampireDownedEntitys);
            }

        }
    }
}
