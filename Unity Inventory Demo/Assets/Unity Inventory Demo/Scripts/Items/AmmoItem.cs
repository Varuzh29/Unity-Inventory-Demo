using UnityEngine;

public class AmmoItem : Item
{
    public WeaponType Type { get; }
    public int Сonsumption { get; }

    public AmmoItem(string name, float weight, Sprite icon, int stackSize, WeaponType type, int consumption) : base(name, weight, icon, stackSize)
    {
        Type = type;
        Сonsumption = consumption;
    }

    public override string ToString()
    {
        return base.ToString() + $", Type: {Type}";
    }
}