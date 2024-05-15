using Bloodstone.API;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using Unity.Collections;
using Unity.Entities;

namespace Bloody.Core.Helper
{
    public static class QueryComponents
    {
        private static Type GetType<T>() => Il2CppType.Of<T>();

        public static unsafe T GetComponentDataAOT<T>(this EntityManager entityManager, Entity entity) where T : unmanaged
        {
            var type = TypeManager.GetTypeIndex(GetType<T>());
            var result = (T*)entityManager.GetComponentDataRawRW(entity, type);

            return *result;
        }

        public static NativeArray<Entity> GetEntitiesByComponentTypes<T1>(EntityQueryOptions queryOption = default, bool includeAll = false)
        {
            EntityQueryOptions options = queryOption;
            if (queryOption == default)
            {
                options = includeAll ? EntityQueryOptions.IncludeAll : EntityQueryOptions.Default;
            }
            
            EntityQueryDesc queryDesc = new EntityQueryDesc
            {
                All = new ComponentType[] {
                new(Il2CppType.Of<T1>(), ComponentType.AccessMode.ReadWrite)
            },
                Options = options
            };

            var query = Core.SystemsCore.EntityManager.CreateEntityQuery(queryDesc);
            var entities = query.ToEntityArray(Allocator.Temp);

            return entities;
        }

        public static NativeArray<Entity> GetEntitiesByComponentTypes<T1, T2>(EntityQueryOptions queryOption = default, bool includeAll = false)
        {
            EntityQueryOptions options = queryOption;
            if (queryOption == default)
            {
                options = includeAll ? EntityQueryOptions.IncludeAll : EntityQueryOptions.Default;
            }

            EntityQueryDesc queryDesc = new EntityQueryDesc
            {
                All = new ComponentType[] {
                new(Il2CppType.Of<T1>(), ComponentType.AccessMode.ReadWrite),
                new(Il2CppType.Of<T2>(), ComponentType.AccessMode.ReadWrite)
            },
                Options = options
            };

            var query = Core.SystemsCore.EntityManager.CreateEntityQuery(queryDesc);
            var entities = query.ToEntityArray(Allocator.Temp);

            return entities;
        }

        public static NativeArray<Entity> GetEntitiesByComponentTypes<T1, T2, T3>(EntityQueryOptions queryOption = default, bool includeAll = false)
        {
            EntityQueryOptions options = queryOption;
            if (queryOption == default)
            {
                options = includeAll ? EntityQueryOptions.IncludeAll : EntityQueryOptions.Default;
            }

            EntityQueryDesc queryDesc = new EntityQueryDesc
            {
                All = new ComponentType[] {
                new(Il2CppType.Of<T1>(), ComponentType.AccessMode.ReadWrite),
                new(Il2CppType.Of<T2>(), ComponentType.AccessMode.ReadWrite),
                new(Il2CppType.Of<T3>(), ComponentType.AccessMode.ReadWrite)
            },
                Options = options
            };

            var query = Core.SystemsCore.EntityManager.CreateEntityQuery(queryDesc);
            var entities = query.ToEntityArray(Allocator.Temp);

            return entities;
        }

        public static void ExploreEntity(Entity entity)
        {
            var sb = new Il2CppSystem.Text.StringBuilder();
            ProjectM.EntityDebuggingUtility.DumpEntity(Core.World, entity, true, sb);
            Core.Logger.LogInfo(sb.ToString());
        }
    }
}
