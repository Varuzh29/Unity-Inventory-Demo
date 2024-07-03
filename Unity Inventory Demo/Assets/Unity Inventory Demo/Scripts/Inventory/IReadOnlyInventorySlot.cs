using System;

public interface IReadOnlyInventorySlot
{
    InventorySlotData InventorySlotData { get; }
    bool IsEmpty { get; }
    event Action<IReadOnlyInventorySlot> DataChanged;
    Item ToItem();
}