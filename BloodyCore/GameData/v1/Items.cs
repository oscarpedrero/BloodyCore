using System;
using System.Collections.Generic;
using System.Linq;
using ProjectM;
using Stunlock.Core;
using Unity.Entities;
using Unity.Collections;
using Bloody.Core.Models.v1;

namespace Bloody.Core.GameData.v1
{
    public class Items
    {
        private Items() { }

        private static Items _instance;
        internal static Items Instance => _instance ??= new Items();

        public ItemModel GetPrefabById(PrefabGUID prefabGuid)
        {
            try
            {
                var entity =  Core.SystemsCore.PrefabCollectionSystem._PrefabLookupMap[prefabGuid];
                return FromEntity(entity);
            }
            catch (Exception ex)
            {
                Core.Logger.LogError(ex);
            }

            return null;
        }

        public ItemModel FromEntity(Entity entity)
        {
            try
            {
                if (entity.Has<ItemData>())
                {
                    return new ItemModel(entity);
                }
            }
            catch (Exception ex)
            {
                Core.Logger.LogError(ex);
            }

            return null;
        }

        private List<ItemModel> _itemPrefabs;
        public List<ItemModel> Prefabs => _itemPrefabs ??= GetItemPrefabs();

        private List<ItemModel> GetItemPrefabs()
        {
            var result = new List<ItemModel>();
            var gameData = Core.World.GetExistingSystemManaged<GameDataSystem>();
            foreach (var prefabEntity in gameData.ItemHashLookupMap.m_HashMapData.GetValueArray(Allocator.Temp))
            {
                var itemModel = FromEntity(prefabEntity.Entity);
                if (itemModel != null)
                {
                    result.Add(itemModel);
                }
            }
            return result;
        }

        private List<ItemModel> _weapons;
        public List<ItemModel> Weapons => _weapons ??= Prefabs.Where(itemModel => itemModel.EquipmentType == EquipmentType.Weapon).ToList();

    }
}