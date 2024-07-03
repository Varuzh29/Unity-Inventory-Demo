using UnityEngine;
using System;

public class ClothingItem : Item
{
	public ClothingType Type { get; }
	public int Protection { get; }

	public ClothingItem(string name, float weight, Sprite icon, int stackSize, int protection, ClothingType type) : base(name, weight, icon, stackSize)
	{
		if (protection < 0)
		{
			throw new ArgumentException("Protection can't be negative!");
		}
		Protection = protection;
		Type = type;
	}

	public override string ToString()
	{
		return base.ToString() + $", Type: {Type}, Protection: {Protection}";
	}
}