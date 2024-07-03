using System.Collections.Generic;
using UnityEngine;

public class RandomLootProvider : ILootProvider
{
    private readonly ItemDatabase _itemDatabase;

    public RandomLootProvider(ItemDatabase itemDatabase)
    {
        _itemDatabase = itemDatabase;
    }

    public List<InventorySlotData> GetLoot()
    {
        List<InventorySlotData> loot = new();
        Item lootItem = _itemDatabase.GetRandomItem();
        int quantity = Random.Range(1, lootItem.StackSize);
        InventorySlotData inventorySlotData = new(lootItem.Name, quantity);
        loot.Add(inventorySlotData);
        return loot;
    }
}