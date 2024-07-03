using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class AddressablesItemsDataProvider : IItemsDataProvider
{
    public async void LoadItems(Action<Dictionary<string, Item>> loaded)
    {
        Dictionary<string, Item> result = new();
        var itemSOs = await Addressables.LoadAssetsAsync<ItemSO>("item", (itemSO) =>
        {
            result[itemSO.name] = itemSO.ToItem();
        }).Task.AsUniTask();
        loaded?.Invoke(result);
    }
}