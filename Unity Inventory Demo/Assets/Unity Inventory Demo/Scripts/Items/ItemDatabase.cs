using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemDatabase
{
    private Dictionary<string, Item> _items;
    public IReadOnlyDictionary<string, Item> Items => _items;

    public ItemDatabase(Dictionary<string, Item> items)
    {
        _items = items;
    }

    public Item GetItem(string name)
    {
        if (_items.TryGetValue(name, out Item item))
        {
            return item;
        }
        throw new KeyNotFoundException($"Item '{name}' not found.");
    }

    public Item GetRandomItem()
    {
        int index = Random.Range(0, _items.Count);
        Item item = _items.ElementAt(index).Value;
        if (item == null)
        {
            Debug.Log($"Item at index {index} is null");
        }
        return item;
    }

    public bool HasItem(string name)
    {
        return _items.ContainsKey(name);
    }

    public override string ToString()
    {
        string result = string.Empty;
        foreach (var item in _items)
        {
            result += $"{item.Value}\n";
        }
        return result == string.Empty ? "Database is empty." : result;
    }
}