using UnityEngine;
using System;

[Serializable]
public abstract class Item 
{
	public string Name { get; }
	public float Weight { get; }
	public Sprite Icon { get; }
	public int StackSize { get; }

	protected Item(string name, float weight, Sprite icon, int stackSize)
	{
		if (string.IsNullOrWhiteSpace(name)) 
		{
			throw new ArgumentException("Name can't be empty or whitespace.");
		}
		if (weight < 0)
		{
			throw new ArgumentException("Weight can't be negative!");
		}
		if (stackSize < 1)
		{
			throw new ArgumentException("Stack size can't be less than 1!");
		}
		Name = name;
		Weight = weight;
		Icon = icon;
		StackSize = stackSize;
	}

    public override string ToString()
    {
        return $"{Name}: Weight: {Weight} kg, StackSize: {StackSize}";
    }
}