﻿namespace Bloody.Core.Models.v1.Data;

public class InventoryItemData
{
    public ItemModel Item { get; set; }
    public int Stacks { get; set; }
    public int Slot { get; set; }
}