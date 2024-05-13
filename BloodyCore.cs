using BepInEx.Logging;
using BloodyCore.Patch.Client;
using BloodyCore.Patch.Server;
using HarmonyLib;
using System;
using Unity.Entities;
using UnityEngine;

namespace BloodyCore
{

    public delegate void OnGameDataInitializedEventHandler(World world);
    public delegate void OnGameDataDestroyedEventHandler();

    public static class BloodyCore
    {

        static BloodyCore()
        {
            Create();
        }

        private static Harmony _harmony;

        private static bool _initialized;
        private static bool _worldDataInitialized;

        public static Systems Systems => _worldDataInitialized ? Systems.Instance : throw new InvalidOperationException(NotInitializedError);

        private const string NotInitializedError = "GameData is not initialized";

        public static bool IsServer = Application.productName == "VRisingServer";
        public static bool IsClient = Application.productName == "VRising";

        internal static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("BloodyCore");

        private static World _world;
        public static World World => _world ?? throw new InvalidOperationException(NotInitializedError);

        public static event OnGameDataInitializedEventHandler OnInitialize;
        public static event OnGameDataDestroyedEventHandler OnDestroy;

        private static void Create()
        {
            if (_initialized)
            {
                return;
            }
            _initialized = true;

            _harmony = new Harmony("BloodyCore");

            if (IsClient)
            {
                _harmony.PatchAll(typeof(OnGameClientDataInitializedPatch));
                OnGameClientDataInitializedPatch.OnGameDataInitialized += OnGameDataInitialized;
                OnGameClientDataInitializedPatch.OnGameDataDestroyed += OnGameDataDestroyed;
            }

            if (IsServer)
            {
                _harmony.PatchAll(typeof(DeathPatch));
                _harmony.PatchAll(typeof(DeathVBloodPatch));
                _harmony.PatchAll(typeof(GameBootstrapPatch));
                _harmony.PatchAll(typeof(OnGameServerDataInitializedPatch));
                _harmony.PatchAll(typeof(ServerBootstrapPatch));
                _harmony.PatchAll(typeof(TraderPurchasePatch));
                _harmony.PatchAll(typeof(UnitSpawnerPatch));
                OnGameServerDataInitializedPatch.OnGameDataInitialized += OnGameDataInitialized;

            }

        }

        internal static void Destroy()
        {
            OnInitialize = null;
            if (IsClient)
            {
                OnGameClientDataInitializedPatch.OnGameDataInitialized -= OnGameDataInitialized;
            }

            if (IsServer)
            {
                OnGameServerDataInitializedPatch.OnGameDataInitialized -= OnGameDataInitialized;
            }

            _harmony.UnpatchSelf();
            _harmony = null;
        }

        private static void OnGameDataDestroyed()
        {
            _world = null;
            _worldDataInitialized = false;
            OnDestroy?.Invoke();
            if (OnDestroy == null)
            {
                return;
            }
            foreach (var hook in OnDestroy.GetInvocationList())
            {
                try
                {
                    hook.DynamicInvoke();
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }
            }
        }

        private static void OnGameDataInitialized(World world)
        {
            _world = world;
            _worldDataInitialized = true;
            if (OnInitialize == null)
            {
                return;
            }
            foreach (var hook in OnInitialize.GetInvocationList())
            {
                try
                {
                    hook.DynamicInvoke(world);
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }
            }
            Logger.LogInfo("GameData initialized");
        }

    }
}
