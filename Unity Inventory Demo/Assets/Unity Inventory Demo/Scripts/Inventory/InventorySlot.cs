using System;
using System.Collections.Generic;

[Serializable]
public class InventorySlot : IReadOnlyInventorySlot
{
    public InventorySlotData InventorySlotData => _inventorySlotData;
    public bool IsEmpty => string.IsNullOrWhiteSpace(_inventorySlotData.Name);
    public event Action<IReadOnlyInventorySlot> DataChanged;
    private readonly ItemDatabase _itemDatabase;
    private InventorySlotData _inventorySlotData;

    public InventorySlot(ItemDatabase itemDatabase, InventorySlotData inventorySlotData)
    {
        _itemDatabase = itemDatabase;
        _inventorySlotData = inventorySlotData;
    }

    public int GetAvailableSpaceFor(string itemName)
    {
        if (!_itemDatabase.HasItem(itemName))
        {
            throw new KeyNotFoundException($"Item '{itemName}' not found.");
        }
        if (IsEmpty)
        {
            return _itemDatabase.GetItem(itemName).StackSize;
        }
        else
        {
            if (itemName != _inventorySlotData.Name)
            {
                return 0;
            }
            return _itemDatabase.GetItem(itemName).StackSize - _inventorySlotData.Quantity;
        }
    }

    public int GetAvailableQuantityFor(string itemName)
    {
        if (!_itemDatabase.HasItem(itemName))
        {
            throw new KeyNotFoundException($"Item '{itemName}' not found.");
        }
        return itemName == _inventorySlotData.Name ? _inventorySlotData.Quantity : 0;
    }

    public int Add(string itemName, int quantity)
    {
        if (!_itemDatabase.HasItem(itemName))
        {
            throw new KeyNotFoundException($"Item '{itemName}' not found.");
        }
        if (quantity < 1)
        {
            throw new ArgumentException("Quantity can't be less than 1.");
        }

        int remaining = quantity;
        int availableSpace = GetAvailableSpaceFor(itemName);

        if (availableSpace > 0)
        {
            int toAdd = Math.Min(availableSpace, remaining);
            _inventorySlotData.Name = itemName;
            _inventorySlotData.Quantity += toAdd;
            remaining -= toAdd;
            DataChanged?.Invoke(this);
        }

        return remaining;
    }

    public int Remove(string itemName, int quantity)
    {
        if (!_itemDatabase.HasItem(itemName))
        {
            throw new KeyNotFoundException($"Item '{itemName}' not found.");
        }
        if (quantity < 1)
        {
            throw new ArgumentException("Quantity can't be less than 1.");
        }

        int remaining = quantity;
        int availableQuantity = GetAvailableQuantityFor(itemName);

        if (availableQuantity > 0)
        {
            int toRemove = Math.Min(availableQuantity, remaining);
            _inventorySlotData.Quantity -= toRemove;
            remaining -= toRemove;
            if (_inventorySlotData.Quantity < 1)
            {
                _inventorySlotData.Name = "";
            }
            DataChanged?.Invoke(this);
        }

        return remaining;
    }

    public void Clear()
    {
        _inventorySlotData.Name = "";
        _inventorySlotData.Quantity = 0;
        DataChanged?.Invoke(this);
    }

    public void Fill()
    {
        _inventorySlotData.Quantity = _itemDatabase.GetItem(_inventorySlotData.Name).StackSize;
        DataChanged?.Invoke(this);
    }

    public Item ToItem()
    {
        try
        {
            return _itemDatabase.GetItem(_inventorySlotData.Name);
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    public void Swap(InventorySlot other)
    {
        (other._inventorySlotData.Name, _inventorySlotData.Name) = (_inventorySlotData.Name, other._inventorySlotData.Name);
        (other._inventorySlotData.Quantity, _inventorySlotData.Quantity) = (_inventorySlotData.Quantity, other._inventorySlotData.Quantity);
        DataChanged?.Invoke(this);
        DataChanged?.Invoke(other);
    }
}