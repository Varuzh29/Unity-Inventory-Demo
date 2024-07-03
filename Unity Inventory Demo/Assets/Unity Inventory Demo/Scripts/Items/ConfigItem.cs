using UnityEngine;

[System.Serializable]
public class ConfigItem
{
    public ItemSO item;
    public int quantity;

    public void Validate()
    {
        quantity = Mathf.Clamp(quantity, 0, item.StackSize);
    }
}