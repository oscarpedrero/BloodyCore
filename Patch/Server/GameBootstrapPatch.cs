using Bloodstone.API;
using HarmonyLib;
using ProjectM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;

namespace BloodyCore.Patch.Server
{
    public delegate void GameBootstrapStartEventHandler();

    public class GameBootstrapPatch
    {

        public static event GameBootstrapStartEventHandler OnStart;

        [HarmonyPatch(typeof(GameBootstrap), nameof(GameBootstrap.Start))]
        [HarmonyPostfix]
        private static void Postfix()
        {
            BloodyCore.Logger.LogDebug($"GameBootstrap.Start");

            try
            {
                OnStart?.Invoke();
            }
            catch (Exception ex)
            {
                BloodyCore.Logger.LogError(ex);
            }
        }
    }
}
