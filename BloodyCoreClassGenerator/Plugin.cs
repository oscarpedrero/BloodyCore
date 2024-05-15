using System;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using ProjectM;
using Unity.Entities;
using Bloody.Core.ClassGenerator.Patch;

namespace Bloody.Core.ClassGenerator
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("gg.deca.Bloodstone")]
    public class Plugin : BasePlugin
    {
        public const string PluginGuid = "Bloody.Core.ClassGenerator";
        public const string PluginName = "Bloody.Core.ClassGenerator";
        public const string PluginVersion = "0.6.0";
        internal static ManualLogSource Logger { get; private set; }
        private static Harmony _harmonyInstance;

        public override void Load()
        {
            Logger = Log;
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginName} is loaded!");
            _harmonyInstance = new Harmony(PluginGuid);
            _harmonyInstance.PatchAll(typeof(ServerEvents));
            /*if (!ClassInjector.IsTypeRegisteredInIl2Cpp<CreateGameDataClassesConsoleCommand>())
            {
                ClassInjector.RegisterTypeInIl2Cpp<CreateGameDataClassesConsoleCommand>();
            }*/


            ServerEvents.OnServerStartupStateChanged += ServerEvents_OnServerStartupStateChanged;
            ServerEvents.OnCoreInitialized += GameDataOk;
        }

        private void GameDataOk(World world)
        {
            Logger.LogInfo($"GameDataOk is loaded!");
            ClassGenerator.GenerateClasses();
        }

        private void ServerEvents_OnServerStartupStateChanged(LoadPersistenceSystemV2 sender, ServerStartupState.State serverStartupState)
        {
            switch (serverStartupState)
            {
                case ServerStartupState.State.None:
                    break;
                case ServerStartupState.State.Waiting:
                    break;
                case ServerStartupState.State.Initializing:
                    break;
                case ServerStartupState.State.SuccessfulStartup:
                    ClassGenerator.GenerateClasses();
                    break;
                case ServerStartupState.State.Failed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(serverStartupState), serverStartupState, null);
            }
        }

        public override bool Unload()
        {
            _harmonyInstance?.UnpatchSelf();
            Logger.LogInfo($"Plugin {PluginGuid} is unloaded!");
            return true;
        }
    }
}
