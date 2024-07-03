using System.Collections.Generic;

public interface IReadOnlyInventory
{
    int SlotCount { get; }
    IReadOnlyList<IReadOnlyInventorySlot> Slots { get; }
    int GetIndexOf(IReadOnlyInventorySlot slot);
}