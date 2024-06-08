using System;
using System.Collections.Generic;
using ProjectM;
using Unity.Entities;

namespace Bloody.Core.Models.v1.Internals
{
    internal static class Extensions
    {
        public static List<T> ToList<T>(this DynamicBuffer<T> bufferList) where T : new()
        {
            if (bufferList.Length == 0)
            {
                return null;
            }

            var result = new List<T>();
            foreach (var item in bufferList)
            {
                result.Add(item);
            }

            return result;
        }

        public static List<T> GetBufferInternal<T>(this EntityManager entityManager, Entity entity) where T : struct
        {
            try
            {
                return entityManager.GetBuffer<T>(entity).ToList();
            }
            catch (Exception ex)
            {
                //Core.Logger.LogDebug(ex.Message);
                return null;
            }
        }

        public static bool HasComponentInternal<T>(this EntityManager entityManager, Entity entity)
        {
            try
            {
                return entityManager.HasComponent<T>(entity);
            }
            catch
            {
                return false;
            }
        }

        public static bool TryGetComponentDataInternal<T>(this EntityManager entityManager, Entity entity, out T value)
            where T : new()
        {
            try
            {
                value = entityManager.GetComponentData<T>(entity);
                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }

        public static T GetManagedComponentDataInternal<T>(this World world, BaseEntityModel entity)
            where T : class
        {
            var prefabGuid = entity.PrefabGUID;
            if (prefabGuid == null)
            {
                return null;
                ;
            }

            var managedDataRegistry = world.GetExistingSystemManaged<GameDataSystem>().ManagedDataRegistry;
            return managedDataRegistry.GetOrDefault<T>(prefabGuid.Value);
        }
    }
}