using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public Sprite Icon;
    public float Weight;
    public int StackSize;

    public abstract Item ToItem();
}