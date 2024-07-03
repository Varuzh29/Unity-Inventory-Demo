using UnityEngine;
using UniRx;
using System;

public class Health
{
	public ReactiveProperty<int> HP { get; private set; }
	public ReactiveProperty<int> MaxHP { get; private set; }
	public IReadOnlyReactiveProperty<bool> IsAlive => HP.Select(hp => hp > 0).ToReactiveProperty();

	public Health(int hp, int maxHp)
	{
		if (hp < 0)
		{
			throw new ArgumentException("HP can't be negative!");
		}
		if (maxHp < hp)
		{
			throw new ArgumentException("MaxHP can't be less than HP!");
		}
		HP = new ReactiveProperty<int>(hp);
		MaxHP = new ReactiveProperty<int>(maxHp);
	}

	public void ApplyDamage(int amount)
	{
		if (IsAlive.Value == false)
		{
			throw new ArgumentException("Can't apply damage if not alive");
		}
		if (amount < 0)
		{
			throw new ArgumentException("Damage amount can't be negative!");
		}
		HP.Value = Mathf.Max(HP.Value - amount, 0);
	}

	public void Heal(int amount)
	{
		if (IsAlive.Value == false)
		{
			throw new ArgumentException("Can't heal if not alive");
		}
		if (amount < 0)
		{
			throw new ArgumentException("Heal amount can't be negative!");
		}
		HP.Value = Mathf.Min(HP.Value + amount, MaxHP.Value);
	}

	public void Reanimate()
	{
		HP.Value = MaxHP.Value;
	}

	public void SetMaxHP(int maxHp)
	{
		if (maxHp < 0)
		{
			throw new ArgumentException("MaxHP can't be negative!");
		}
		if (HP.Value > maxHp)
		{
			HP.Value = maxHp;
		}
		MaxHP.Value = maxHp;
	}

	public override string ToString()
	{
		return $"HP: {HP.Value}/{MaxHP.Value}";
	}
}