using System;
using System.Collections.Generic;
using ProjectM;
using Stunlock.Core;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Bloody.Core.Models;
using Bloody.Core.Models.Internals;

namespace Bloody.Core.Utils
{
    public static class ExtensionMethods
    {
        public static string GetPrefabName(this PrefabGUID prefabGuid)
        {
            try
            {
                return Core.SystemsCore.PrefabCollectionSystem.PrefabLookupMap[prefabGuid].ToString();
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<BaseEntityModel> ToEnumerable(this EntityQuery entityQuery)
        {
            var entities = entityQuery.ToEntityArray(Allocator.Temp);
            foreach (var entity in entities)
            {
                yield return new BaseEntityModel(Core.World, entity);
            }
        }

        //private static IEnumerable<T> AsEnumerable<T>(this EntityQuery entityQuery) where T : EntityModel
        //{
        //    var entities = entityQuery.ToEntityArray(Allocator.Temp);
        //    foreach (var entity in entities)
        //    {
        //        if (typeof(T) == typeof(NpcModel))
        //        {
        //            yield return new NpcModel(entity) as T;
        //        }
        //        if (typeof(T) == typeof(UserModel))
        //        {
        //            yield return new UserModel(entity) as T;
        //        }
        //        if (typeof(T) == typeof(ItemModel))
        //        {
        //            yield return new ItemModel(entity) as T;
        //        }
        //    }
        //}

        public static IEnumerable<NpcModel> AsNpcs(this EntityQuery entityQuery) 
        {
            var entities = entityQuery.ToEntityArray(Allocator.Temp);
            foreach (var entity in entities)
            {
                yield return new NpcModel(entity);
            }
        }

        public static DateTime ToDateTime(this long unixDateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return start.AddMilliseconds(unixDateTime);
        }

        public static double Distance(this float3 pos1, float3 pos2)
        {
            return Math.Sqrt((Math.Pow(pos1.x - pos2.x, 2) + Math.Pow(pos1.z - pos2.z, 2)));
        }
    }
}