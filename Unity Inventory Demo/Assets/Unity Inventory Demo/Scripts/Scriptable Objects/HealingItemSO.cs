using UnityEngine;

[CreateAssetMenu(fileName = "New Healing Item", menuName = "Inventory/Items/Healing Item")]
public class HealingItemSO : ItemSO
{
    public int HealAmount;

    public override Item ToItem()
    {
        return new HealingItem(name, Weight, Icon, StackSize, HealAmount);
    }
}