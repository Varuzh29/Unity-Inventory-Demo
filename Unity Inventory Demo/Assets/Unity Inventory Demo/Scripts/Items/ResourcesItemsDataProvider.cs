using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesItemsDataProvider : IItemsDataProvider
{
    public void LoadItems(Action<Dictionary<string, Item>> loaded)
    {
        ItemSO[] itemSOs = Resources.LoadAll<ItemSO>("Items");
        Dictionary<string, Item> result = new();
        foreach (var itemSO in itemSOs)
        {
            result[itemSO.name] = itemSO.ToItem();
        }
        loaded?.Invoke(result);
    }
}