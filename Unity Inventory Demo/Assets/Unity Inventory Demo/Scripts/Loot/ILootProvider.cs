using System.Collections.Generic;

public interface ILootProvider
{
    List<InventorySlotData> GetLoot();
}