using System;
using System.Collections.Generic;
using ProjectM;
using Stunlock.Core;
using Unity.Entities;
using Unity.Mathematics;
using Bloody.Core.Models.v1.Base;

namespace Bloody.Core.Models.v1
{
    public partial class NpcModel : EntityModel
    {
        private static readonly HashSet<UnitCategory> _npcCategories = new()
        {
            UnitCategory.Beast,
            UnitCategory.Demon,
            UnitCategory.Human,
            UnitCategory.Undead,
            UnitCategory.Mechanical
        };

        internal NpcModel(Entity entity) : base(entity)
        {
            var entityCategory = Internals.EntityCategory;
            if (entityCategory == null || entityCategory.Value.MainCategory != MainEntityCategory.Unit || !_npcCategories.Contains(entityCategory.Value.UnitCategory))
            {
                throw new Exception("Given entity is not a NPC.");
            }
        }

#pragma warning disable CS0108 
        public PrefabGUID PrefabGUID => Internals.PrefabGUID ?? new PrefabGUID();
#pragma warning restore CS0108 
        public bool IsDead => Internals.DeathBuff.HasValue;
        public float LifeTime => Internals.LifeTime?.Duration ?? 0;
        public float3 Position => Internals.LocalToWorld?.Position ?? new float3();
    }
}