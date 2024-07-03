using UnityEngine;
using System;

public class HealingItem : Item
{
	public int HealAmount { get; private set; }

	public HealingItem(string name, float weight, Sprite icon, int stackSize, int healAmount) : base(name, weight, icon, stackSize)
	{
		if (healAmount < 0)
		{
			throw new ArgumentException("Heal amount can't be negative!");
		}
		HealAmount = healAmount;
	}

    public override string ToString()
    {
        return base.ToString() + $", HealAmount: {HealAmount}";
    }
}