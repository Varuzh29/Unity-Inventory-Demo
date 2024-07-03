using System;

[Serializable]
public struct InventorySlotData
{
    public string Name;
    public int Quantity;

    public InventorySlotData(string name, int quantity)
    {
        Name = name;
        Quantity = quantity;
    }
}