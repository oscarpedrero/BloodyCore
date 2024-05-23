using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Bloody.Core.API.v1;
using Bloody.Core.Helper.v1;
using Bloody.Core.Patch.Client;
using Bloody.Core.Patch.Server;
using HarmonyLib;
using System;
using Unity.Entities;
using UnityEngine;

namespace Bloody.Core
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("gg.deca.Bloodstone")]
    public class Core: BasePlugin
    {
        internal static Core Instance { get; private set; }

        private static Harmony _harmony;

        private static bool _initialized;
        internal static bool _worldDataInitialized;

        public static SystemsCore SystemsCore => _worldDataInitialized ? SystemsCore.Instance : throw new InvalidOperationException(NotInitializedError);

        internal const string NotInitializedError = "Core is not initialized";

        public static bool IsServer = Application.productName == "VRisingServer";
        public static bool IsClient = Application.productName == "VRising";

        internal static ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource("Bloody.Core");

        internal static Helper.v1.Logger Logger = new(Log);

        private static World _world;
        public static World World => _world ?? throw new InvalidOperationException(NotInitializedError);

        public static EventsHandlerSystem EventHandlerSystem;

        public Core() : base()
        {
            Instance = this;
        }

        public override void Load()
        {
            if (_initialized)
            {
                return;
            }
            _initialized = true;

            _harmony = new Harmony("Bloody.Core");

            if (IsClient)
            {
                Logger.LogInfo($"Load Systems for Client");
                _harmony.PatchAll(typeof(OnGameClientDataInitializedPatch));
                OnGameClientDataInitializedPatch.OnCoreInitialized += OnCoreInitialized;
                OnGameClientDataInitializedPatch.OnCoreDestroyed += OnCoreDestroyed;
            }

            if (IsServer)
            {
                Logger.LogInfo($"Load Systems for Server");
                _harmony.PatchAll(typeof(DeathPatch));
                _harmony.PatchAll(typeof(DeathVBloodPatch));
                _harmony.PatchAll(typeof(GameBootstrapPatch));
                _harmony.PatchAll(typeof(ServerBootstrapPatch));
                _harmony.PatchAll(typeof(TraderPurchasePatch));
                _harmony.PatchAll(typeof(UnitSpawnerPatch));
                _harmony.PatchAll(typeof(SaveSystemPatch));

                _harmony.PatchAll(typeof(OnGameServerDataInitializedPatch));
                OnGameServerDataInitializedPatch.OnCoreInitialized += OnCoreInitialized;

            }

            EventHandlerSystem = new EventsHandlerSystem();

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} is loaded!");
        }

        internal static void Destroy()
        {
            _harmony.UnpatchSelf();
            _harmony = null;
        }

        internal static void OnCoreDestroyed()
        {
            _world = null;
            _worldDataInitialized = false;
        }

        internal static void OnCoreInitialized(World world)
        {
            Logger.LogInfo($"OnCoreInitialized is loaded!");
            _world = world;
            _worldDataInitialized = true;
        }
    }
}
