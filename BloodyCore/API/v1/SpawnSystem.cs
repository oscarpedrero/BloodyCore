using Bloodstone.API;
using Bloody.Core.Patch.v1.Server;
using ProjectM;
using Stunlock.Core;
using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Bloody.Core.API.v1
{
    public class SpawnSystem
    {

        internal const int DEFAULT_MINRANGE = 1;
        internal const int DEFAULT_MAXRANGE = 1;

        public static void SpawnUnitWithCallback(Entity user, PrefabGUID unit, float2 position, float duration, Action<Entity> postActions)
        {
            Entity empty_entity = new Entity();
            var translation = Core.SystemsCore.EntityManager.GetComponentData<Translation>(user);
            var f3pos = new float3(position.x, translation.Value.y, position.y); // TODO: investigate this copypasta
            var usus = Core.SystemsCore.UnitSpawnerUpdateSystem;
            UnitSpawnerPatch.EnabledCallBack = true;
            var durationKey = NextKey();
            usus.SpawnUnit(empty_entity, unit, f3pos, 1, DEFAULT_MINRANGE, DEFAULT_MAXRANGE, durationKey);
            UnitSpawnerPatch.PostActions.Add(durationKey, (duration, postActions));
        }

        internal static long NextKey()
        {
            System.Random r = new();
            long key;
            int breaker = 5;
            do
            {
                key = r.NextInt64(10000) * 3;
                breaker--;
                if (breaker < 0)
                {
                    throw new Exception($"Failed to generate a unique key for UnitSpawnerService");
                }
            } while (UnitSpawnerPatch.PostActions.ContainsKey(key));
            return key;
        }
    }
}
