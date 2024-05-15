﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bloodstone.API;
using ProjectM;
using Stunlock.Core;
using Unity.Entities;
using Bloody.Core.Models;
using Unity.Collections;

namespace Bloody.Core
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
                var entity = Core.SystemsCore.PrefabCollectionSystem.PrefabLookupMap[prefabGuid];
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
                Core.Logger.LogInfo("FromEntity");
                if (entity.Has<ItemData>())
                {
                    Core.Logger.LogInfo("FromEntity 2");
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
            Core.Logger.LogInfo("GetItemPrefabs");
            var result = new List<ItemModel>();
            Core.Logger.LogInfo("GetItemPrefabs 2");
            var gameData = VWorld.Server.GetExistingSystemManaged<GameDataSystem>();
            Core.Logger.LogInfo("GetItemPrefabs 3");
            foreach (var prefabEntity in gameData.ItemHashLookupMap.GetValueArray(Allocator.Temp))
            {
                Core.Logger.LogInfo("GetItemPrefabs 4");
                var itemModel = FromEntity(prefabEntity.Entity);
                Core.Logger.LogInfo("GetItemPrefabs 5");
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