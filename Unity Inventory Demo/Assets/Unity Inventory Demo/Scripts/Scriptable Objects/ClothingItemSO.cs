using UnityEngine;

[CreateAssetMenu(fileName = "New Clothing Item", menuName = "Inventory/Items/Clothing Item")]
public class ClothingItemSO : ItemSO
{
    public int Protection;
    public ClothingType Type;

    public override Item ToItem()
    {
        return new ClothingItem(name, Weight, Icon, StackSize, Protection, Type);
    }
}