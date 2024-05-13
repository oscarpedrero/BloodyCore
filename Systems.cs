using ProjectM;
using Stunlock.Core;
using Unity.Entities;

namespace BloodyCore
{
    public class Systems
    {
        private Systems() { }

        private static Systems _instance;
        internal static Systems Instance => _instance ??= new Systems();
        public EntityManager EntityManager => BloodyCore.World.EntityManager;
        public PrefabCollectionSystem PrefabCollectionSystem => BloodyCore.World.GetExistingSystemManaged<PrefabCollectionSystem>();
        public GameDataSystem GameDataSystem => BloodyCore.World.GetExistingSystemManaged<GameDataSystem>();
        public ManagedDataRegistry ManagedDataRegistry => GameDataSystem.ManagedDataRegistry;
        public DebugEventsSystem DebugEventsSystem => BloodyCore.World.GetExistingSystemManaged<DebugEventsSystem>();
        public UnitSpawnerUpdateSystem UnitSpawnerUpdateSystem => BloodyCore.World.GetExistingSystemManaged<UnitSpawnerUpdateSystem>();
    }
}
