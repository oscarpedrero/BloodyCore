using ProjectM;
using ProjectM.Scripting;
using Stunlock.Core;
using Unity.Entities;

namespace Bloody.Core
{
    public class SystemsCore
    {
        public SystemsCore() { }

        private static SystemsCore _instance;
        internal static SystemsCore Instance => _instance ??= new SystemsCore();
        public EntityManager EntityManager => Core.World.EntityManager;
        public PrefabCollectionSystem PrefabCollectionSystem => Core.World.GetExistingSystemManaged<PrefabCollectionSystem>();
        public GameDataSystem GameDataSystem => Core.World.GetExistingSystemManaged<GameDataSystem>();
        public ManagedDataRegistry ManagedDataRegistry => GameDataSystem.ManagedDataRegistry;
        public DebugEventsSystem DebugEventsSystem => Core.World.GetExistingSystemManaged<DebugEventsSystem>();
        public UnitSpawnerUpdateSystem UnitSpawnerUpdateSystem => Core.World.GetExistingSystemManaged<UnitSpawnerUpdateSystem>();
        public ServerScriptMapper ServerScriptMapper => Core.World.GetExistingSystemManaged<ServerScriptMapper>();
    }
}
