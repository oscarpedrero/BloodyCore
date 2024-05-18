using BepInEx.Logging;
using Bloody.Core.API;
using Bloody.Core.Helper;
using Bloody.Core.Patch.Client;
using Bloody.Core.Patch.Server;
using HarmonyLib;
using ProjectM;
using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using static ProjectM.Metrics;

namespace Bloody.Core
{



    public static class Core
    {

        static Core()
        {
            Create();
        }

        private static Harmony _harmony;

        private static bool _initialized;
        private static bool _worldDataInitialized;

        public static SystemsCore SystemsCore => _worldDataInitialized ? SystemsCore.Instance : throw new InvalidOperationException(NotInitializedError);
        public static Users Users => _worldDataInitialized ? Users.Instance : throw new InvalidOperationException(NotInitializedError);
        public static Items Items => _worldDataInitialized ? Items.Instance : throw new InvalidOperationException(NotInitializedError);
        public static Npcs Npcs => _worldDataInitialized ? Npcs.Instance : throw new InvalidOperationException(NotInitializedError);

        private const string NotInitializedError = "Core is not initialized";

        public static bool IsServer = Application.productName == "VRisingServer";
        public static bool IsClient = Application.productName == "VRising";

        internal static ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource("Bloody.Core");

        internal static Helper.Logger Logger = new(Log);

        private static World _world;
        public static World World => _world ?? throw new InvalidOperationException(NotInitializedError);

        public static EventsHandlerSystem EventHandlerSystem;

        private static void Create()
        {
            if (_initialized)
            {
                return;
            }
            _initialized = true;

            _harmony = new Harmony("Bloody.Core");

            if (IsClient)
            {
                _harmony.PatchAll(typeof(OnGameClientDataInitializedPatch));
                OnGameClientDataInitializedPatch.OnCoreInitialized += OnCoreInitialized;
                OnGameClientDataInitializedPatch.OnCoreDestroyed += OnCoreDestroyed;
            }

            if (IsServer)
            {
                _harmony.PatchAll(typeof(OnGameServerDataInitializedPatch));
                _harmony.PatchAll(typeof(DeathPatch));
                _harmony.PatchAll(typeof(DeathVBloodPatch));
                _harmony.PatchAll(typeof(GameBootstrapPatch));
                _harmony.PatchAll(typeof(ServerBootstrapPatch));
                _harmony.PatchAll(typeof(TraderPurchasePatch));
                _harmony.PatchAll(typeof(UnitSpawnerPatch));
                _harmony.PatchAll(typeof(ActionSchedulerPatch));
                _harmony.PatchAll(typeof(SaveSystemPatch));
                OnGameServerDataInitializedPatch.OnCoreInitialized += OnCoreInitialized;

            }

            EventHandlerSystem = new EventsHandlerSystem();

            Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} is loaded!");

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
            _world = world;
            _worldDataInitialized = true;
        }
    }
}
