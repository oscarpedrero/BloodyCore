using Bloody.Core.API.v1;
using HarmonyLib;
using ProjectM;

namespace Bloody.Core.Patch.Server
{
    [HarmonyPatch]
    internal class SaveSystemPatch
    {
        internal static event SaveWorldEventHandler OnSaveWorld;

        [HarmonyPatch(typeof(TriggerPersistenceSaveSystem), nameof(TriggerPersistenceSaveSystem.TriggerSave))]
        public static void Prefix()
        {
            OnSaveWorld?.Invoke();
        }
    }
}
