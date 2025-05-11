using Bloody.Core.API.v1;
using HarmonyLib;
using ProjectM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;

namespace Bloody.Core.Patch.Server
{


    internal class GameBootstrapPatch
    {

        internal static event GameBootstrapStartEventHandler OnStart;

        [HarmonyPatch(typeof(GameBootstrap), nameof(GameBootstrap.Start))]
        [HarmonyPostfix]
        private static void Postfix()
        {
            //Core.Logger.LogDebug($"GameBootstrap.Start");

            try
            {
                OnStart?.Invoke();
            }
            catch (Exception ex)
            {
                Core.Logger.LogError(ex);
            }
        }
    }
}
