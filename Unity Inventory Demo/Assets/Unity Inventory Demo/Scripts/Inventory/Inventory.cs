using System;
using System.Collections.Generic;
using UniRx;

public class Inventory : IReadOnlyInventory
{
    public ItemDatabase _itemDatabase { get; }
    private List<InventorySlot> _slots;
    public int SlotCount => _slots.Count;
    public IReadOnlyList<IReadOnlyInventorySlot> Slots => _slots;
    public event Action DataChanged;

    public Inventory(ItemDatabase itemDatabase, int slotCount)
    {
        if (slotCount < 1)
        {
            throw new ArgumentException("Slot count can't be less than 1.");
        }
        _itemDatabase = itemDatabase;
        _slots = new();
        InitializeSlots(slotCount);
    }

    public void SetInventoryData(List<InventorySlotData> inventorySlotsData)
    {
        for (int i = 0; i < inventorySlotsData.Count; i++)
        {
            if (!string.IsNullOrWhiteSpace(inventorySlotsData[i].Name))
            {
                _slots[i].Add(inventorySlotsData[i].Name, inventorySlotsData[i].Quantity);
            }
        }
        DataChanged?.Invoke();
    }

    private void InitializeSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            _slots.Add(new InventorySlot(_itemDatabase, new InventorySlotData("", 0)));
        }
    }

    /// <summary>
    /// Tries to add the specified quantity of an item to the inventory.
    /// If the inventory is full, the remaining quantity is returned.
    /// </summary>
    /// <param name="name">The name of the item to add.</param>
    /// <param name="quantity">The quantity of the item to add. Default is 1.</param>
    /// <returns>The remaining quantity that couldn't be added.</returns>
    /// <exception cref="KeyNotFoundException">The item is not found in the database.</exception>
    /// <exception cref="ArgumentException">The quantity is less than 1.</exception>
    public int Add(string name, int quantity = 1)
    {
        // Check if the item exists in the database
        if (!_itemDatabase.HasItem(name))
        {
            throw new KeyNotFoundException($"Item '{name}' not found.");
        }

        // Check if the quantity is valid
        if (quantity < 1)
        {
            throw new ArgumentException("Quantity can't be less than 1.");
        }

        int remaining = quantity;
        List<InventorySlot> emptySlots = new();

        // Try to add to slots containing the same item
        foreach (var slot in _slots)
        {
            if (remaining < 1) break;

            if (slot.IsEmpty)
            {
                emptySlots.Add(slot);
                continue;
            }

            if (slot.InventorySlotData.Name == name)
            {
                remaining = slot.Add(name, remaining);
            }
        }

        // Add to empty slots if there are any remaining quantity or empty slots
        foreach (var emptySlot in emptySlots)
        {
            if (remaining < 1) break;

            remaining = emptySlot.Add(name, remaining);
        }

        DataChanged?.Invoke();
        return remaining;
    }

    public int AddAt(int index, string name, int quantity)
    {
        if (index < 0 || index >= _slots.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        int result = _slots[index].Add(name, quantity);
        DataChanged?.Invoke();
        return result;
    }

    /// <summary>
    /// Removes the specified quantity of an item from the inventory.
    /// If quantity is not enough, the remaining quantity is returned.
    /// </summary>
    /// <param name="name">The name of the item to remove.</param>
    /// <param name="quantity">The quantity of the item to remove. Default is 1.</param>
    /// <returns>Remaining quantity that couldn't be removed.</returns>
    /// <exception cref="KeyNotFoundException">The item is not found in the database.</exception>
    /// <exception cref="ArgumentException">The quantity is less than 1.</exception>
    public int Remove(string name, int quantity = 1)
    {
        // Check if the item exists in the database
        if (!_itemDatabase.HasItem(name))
        {
            throw new KeyNotFoundException($"Item '{name}' not found.");
        }

        // Check if the quantity is valid
        if (quantity < 1)
        {
            throw new ArgumentException("Quantity can't be negative.");
        }

        int remaining = quantity;

        foreach (var slot in _slots)
        {
            if (remaining < 1) break;

            remaining = slot.Remove(name, remaining);
        }

        DataChanged?.Invoke();
        return remaining;
    }

    public int RemoveFromSlot(int index, int quantity)
    {
        if (index < 0 || index >= _slots.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        string itemName = _slots[index]?.InventorySlotData.Name;
        int result = _slots[index].Remove(itemName, quantity);
        DataChanged?.Invoke();
        return result;
    }

    public void FillSlot(int index)
    {
        if (index < 0 || index >= _slots.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        _slots[index].Fill();
        DataChanged?.Invoke();
    }

    public bool ClearSlot(int index)
    {
        if (index < 0 || index >= _slots.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        if (_slots[index].IsEmpty)
        {
            throw new InvalidOperationException("Slot is empty.");
        }

        _slots[index].Clear();
        DataChanged?.Invoke();
        return _slots[index].IsEmpty;
    }

    public void ClearAll()
    {
        foreach (var slot in _slots)
        {
            slot.Clear();
        }
        DataChanged?.Invoke();
    }

    public int GetIndexOf(IReadOnlyInventorySlot slot)
    {
        return _slots.IndexOf((InventorySlot)slot);
    }

    public int MoveItem(int fromIndex, int toIndex)
    {
        if (fromIndex == toIndex)
        {
            return 0;
        }
        if (fromIndex < 0 || fromIndex >= _slots.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(fromIndex));
        }
        if (toIndex < 0 || toIndex >= _slots.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(toIndex));
        }
        string itemToMove = _slots[fromIndex].InventorySlotData.Name;
        if (string.IsNullOrWhiteSpace(itemToMove)) return 0;
        int quantityToMove = _slots[fromIndex].InventorySlotData.Quantity;
        int notMovedQuantity = _slots[toIndex].Add(itemToMove, quantityToMove);
        int movedQuantity = quantityToMove - notMovedQuantity;
        if (movedQuantity > 0)
        {
            _slots[fromIndex].Remove(itemToMove, movedQuantity);
        }
        else
        {
            _slots[fromIndex].Swap(_slots[toIndex]);
        }
        DataChanged?.Invoke();
        return notMovedQuantity;
    }

    public int GetAvailableQuantityFor(string name)
    {
        int availableQuantity = 0;
        foreach (var slot in _slots)
        {
            availableQuantity += slot.GetAvailableQuantityFor(name);
        }
        return availableQuantity;
    }

    public int GetAvailableSpaceFor(string name)
    {
        int availableSpace = 0;
        foreach (var slot in _slots)
        {
            availableSpace += slot.GetAvailableSpaceFor(name);
        }
        return availableSpace;
    }

    public bool HasItem(string itemName, int quantity = 1)
    {
        foreach (var slot in _slots)
        {
            if (slot.InventorySlotData.Name == itemName && slot.InventorySlotData.Quantity >= quantity)
            {
                return true;
            }
        }
        return false;
    }

    public override string ToString()
    {
        string result = $"Slot Inventory [Slots: {_slots.Count}]: \n";
        string items = string.Empty;
        foreach (var slot in _slots)
        {
            if (!slot.IsEmpty)
            {
                items += $"{slot.InventorySlotData.Name}:{slot.InventorySlotData.Quantity}\n";
            }
        }
        result += string.IsNullOrWhiteSpace(items) ? "[Empty]" : items;
        return result;
    }

    public List<InventorySlotData> GetInventoryData()
    {
        List<InventorySlotData> inventorySlotsData = new();
        foreach (var slot in _slots)
        {
            inventorySlotsData.Add(slot.InventorySlotData);
        }
        return inventorySlotsData;
    }
}