using Bloody.Core.Patch.v1.Client;
using Bloody.Core.Patch.v1.Server;
using ProjectM;
using Stunlock.Network;
using System;
using Unity.Collections;
using Unity.Entities;

namespace Bloody.Core.API.v1
{
    public delegate void OnGameDataInitializedEventHandler(World world);
    public delegate void OnGameDataDestroyedEventHandler();
    public delegate void DeathEventHandler(DeathEventListenerSystem sender, NativeArray<DeathEvent> deathEvents);
    public delegate void DeathVbloodEventHandler(VBloodSystem sender, NativeList<VBloodConsumed> deathEvents);
    public delegate void GameBootstrapStartEventHandler();
    public delegate void OnUserConnectedEventHandler(ServerBootstrapSystem sender, NetConnectionId netConnectionId);
    public delegate void OnUserDisconnectedEventHandler(ServerBootstrapSystem sender, NetConnectionId netConnectionId, ConnectionStatusChangeReason connectionStatusReason, string extraData);
    public delegate void TraderPurchaseEventHandler(NativeArray<Entity> entities);
    public delegate void OnUnitSpawnedEventHandler(NativeArray<Entity> entities);
    public delegate void VampireDownedHandler(VampireDownedServerEventSystem sender, NativeArray<Entity> deathEvents);
    public delegate void SaveWorldEventHandler();

    public class EventsHandlerSystem
    {
        public static event OnGameDataInitializedEventHandler OnInitialize;
        public static event OnGameDataDestroyedEventHandler OnDestroy;
        public static event DeathVbloodEventHandler OnDeathVBlood;
        public static event DeathEventHandler OnDeath;
        public static event GameBootstrapStartEventHandler OnStart;
        public static event OnUserConnectedEventHandler OnUserConnected;
        public static event OnUserDisconnectedEventHandler OnUserDisconnected;
        public static event TraderPurchaseEventHandler OnTraderPurchase;
        public static event OnUnitSpawnedEventHandler OnUnitSpawned;
        public static event VampireDownedHandler OnVampireDowned;
        public static event SaveWorldEventHandler OnSaveWorld;

        static EventsHandlerSystem()
        {
            if (Core.IsClient)
            {
                OnGameClientDataInitializedPatch.OnCoreInitialized += OnCoreInitialized;
                OnGameClientDataInitializedPatch.OnCoreDestroyed += OnCoreDestroyed;
            }

            if (Core.IsServer)
            {
                OnGameServerDataInitializedPatch.OnCoreInitialized += OnCoreInitialized;
                DeathVBloodPatch.OnDeathVBlood += OnDeathVBloodInvoke;
                DeathPatch.OnDeath += OnDeathInvoke;
                GameBootstrapPatch.OnStart += OnStartInvoke;
                ServerBootstrapPatch.OnUserConnected += OnUserConnectedInvoke;
                ServerBootstrapPatch.OnUserDisconnected += OnUserDisconnectedInvoke;
                TraderPurchasePatch.OnTraderPurchase += OnTraderPurchaseInvoke;
                UnitSpawnerPatch.OnUnitSpawned += OnUnitSpawnedInvoke;
                VampireDownedPatch.OnVampireDowned += OnVampireDownedInvoke;
                SaveSystemPatch.OnSaveWorld += OnSaveWorldInvoke;
            }
        }

        private static void OnSaveWorldInvoke()
        {
            if (OnSaveWorld == null)
            {
                return;
            }
            foreach (var hook in OnSaveWorld.GetInvocationList())
            {
                try
                {
                    hook.DynamicInvoke();
                }
                catch (Exception e)
                {
                    Core.Logger.LogError(e);
                }
            }
            Core.Logger.LogDebug("OnSaveWorld Invoke");
        }

        private static void OnVampireDownedInvoke(VampireDownedServerEventSystem sender, NativeArray<Entity> deathEvents) // TODO: Review why dont run
        {
            if (OnVampireDowned == null)
            {
                return;
            }
            foreach (var hook in OnVampireDowned.GetInvocationList())
            {
                try
                {
                    hook.DynamicInvoke(sender, deathEvents);
                }
                catch (Exception e)
                {
                    Core.Logger.LogError(e);
                }
            }
            Core.Logger.LogDebug("OnVampireDowned Invoke");
        }

        private static void OnUnitSpawnedInvoke(NativeArray<Entity> entities)
        {
            if (OnUnitSpawned == null)
            {
                return;
            }
            foreach (var hook in OnUnitSpawned.GetInvocationList())
            {
                try
                {
                    hook.DynamicInvoke(entities);
                }
                catch (Exception e)
                {
                    Core.Logger.LogError(e);
                }
            }
            Core.Logger.LogDebug("OnUnitSpawned Invoke");
        }

        private static void OnTraderPurchaseInvoke(NativeArray<Entity> entities)
        {
            if (OnTraderPurchase == null)
            {
                return;
            }
            foreach (var hook in OnTraderPurchase.GetInvocationList())
            {
                try
                {
                    hook.DynamicInvoke(entities);
                }
                catch (Exception e)
                {
                    Core.Logger.LogError(e);
                }
            }
            Core.Logger.LogDebug("OnTraderPurchase Invoke");
        }

        private static void OnUserDisconnectedInvoke(ServerBootstrapSystem sender, NetConnectionId netConnectionId, ConnectionStatusChangeReason connectionStatusReason, string extraData)
        {
            if (OnUserDisconnected == null)
            {
                return;
            }
            foreach (var hook in OnUserDisconnected.GetInvocationList())
            {
                try
                {
                    hook.DynamicInvoke(sender, netConnectionId, connectionStatusReason, extraData);
                }
                catch (Exception e)
                {
                    Core.Logger.LogError(e);
                }
            }
            Core.Logger.LogDebug("OnUserDisconnected Invoke");
        }

        private static void OnUserConnectedInvoke(ServerBootstrapSystem sender, NetConnectionId netConnectionId)
        {
            if (OnUserConnected == null)
            {
                return;
            }
            foreach (var hook in OnUserConnected.GetInvocationList())
            {
                try
                {
                    hook.DynamicInvoke(sender, netConnectionId);
                }
                catch (Exception e)
                {
                    Core.Logger.LogError(e);
                }
            }
            Core.Logger.LogDebug("OnUserConnected Invoke");
        }

        private static void OnStartInvoke()
        {
            if (OnStart == null)
            {
                return;
            }
            foreach (var hook in OnStart.GetInvocationList())
            {
                try
                {
                    hook.DynamicInvoke();
                }
                catch (Exception e)
                {
                    Core.Logger.LogError(e);
                }
            }
            Core.Logger.LogDebug("OnStart Invoke");
        }

        private static void OnDeathInvoke(DeathEventListenerSystem sender, NativeArray<DeathEvent> deathEvents)
        {
            if (OnDeath == null)
            {
                return;
            }
            foreach (var hook in OnDeath.GetInvocationList())
            {
                try
                {
                    hook.DynamicInvoke(sender, deathEvents);
                }
                catch (Exception e)
                {
                    Core.Logger.LogError(e);
                }
            }
            Core.Logger.LogDebug("OnDeath Invoke");
        }

        private static void OnDeathVBloodInvoke(VBloodSystem sender, NativeList<VBloodConsumed> deathEvents)
        {
            if (OnDeathVBlood == null)
            {
                return;
            }
            foreach (var hook in OnDeathVBlood.GetInvocationList())
            {
                try
                {
                    hook.DynamicInvoke(sender, deathEvents);
                }
                catch (Exception e)
                {
                    Core.Logger.LogError(e);
                }
            }
            Core.Logger.LogDebug("OnDeathVBlood Invoke");
        }


        internal static void Destroy()
        {
            OnInitialize = null;
            if (Core.IsClient)
            {
                OnGameClientDataInitializedPatch.OnCoreInitialized -= OnCoreInitialized;
            }

            if (Core.IsServer)
            {
                OnGameServerDataInitializedPatch.OnCoreInitialized -= OnCoreInitialized;
            }

            Core.Destroy();
        }

        private static void OnCoreDestroyed()
        {
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
                    Core.Logger.LogError(e);
                }
            }
            Core.OnCoreDestroyed();
        }

        private static void OnCoreInitialized(World world)
        {

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
                    Core.Logger.LogError(e);
                }
            }
            Core.OnCoreInitialized(world);
            Core.Logger.LogDebug("Core initialized");
        }
    }
}
