using System.Collections.Generic;
using System.Linq;
using ProjectM;
using Unity.Entities;
using Bloody.Core.Models.v1.Internals;
using Bloody.Core.Models.v1.Data;

namespace Bloody.Core.Models.v1;

public class InventoryModel
{
    private readonly CharacterModel _characterModel;

    internal InventoryModel(CharacterModel characterModel)
    {
        _characterModel = characterModel;
    }

    public List<InventoryItemData> Items => GetInventoryItems();

    private List<InventoryItemData> GetInventoryItems()
    {
        InventoryUtilities.TryGetInventoryEntity(Core.World.EntityManager, _characterModel.Entity,
            out var inventoryEntity);

        var inventory = new BaseEntityModel(Core.World, inventoryEntity);
        var items = inventory.InventoryBuffers;
        if (items == null)
        {
            return new List<InventoryItemData>();
        }

        return items.Select((i, index) =>
        {
            var itemPrefabEntity = i.ItemType.GuidHash == 0 ? Entity.Null : Core.SystemsCore.PrefabCollectionSystem.PrefabLookupMap[i.ItemType];
            return new InventoryItemData { Item = new ItemModel(itemPrefabEntity), Stacks = i.Amount, Slot = index };
        }).ToList();
    }
}