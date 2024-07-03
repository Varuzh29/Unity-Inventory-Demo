using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Item", menuName = "Inventory/Items/Ammo Item")]
public class AmmoItemSO : ItemSO
{
    public WeaponType Type;
    public int Сonsumption;

    public override Item ToItem()
    {
        return new AmmoItem(name, Weight, Icon, StackSize, Type, Сonsumption);
    }
}