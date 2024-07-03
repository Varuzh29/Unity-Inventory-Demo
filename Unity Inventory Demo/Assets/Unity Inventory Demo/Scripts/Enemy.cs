using System.Collections.Generic;

public class Enemy
{
    public Health Health { get; }
    private bool _damageHead;
    public ILootProvider LootProvider { get; }

    public Enemy(int hp, int maxHp, ILootProvider lootProvider)
    {
        LootProvider = lootProvider;
        Health = new Health(hp, maxHp);
    }

    public DamageDirection GetDamageDirection()
    {
        _damageHead = !_damageHead;
        return _damageHead ? DamageDirection.Head : DamageDirection.Torso;
    }

    public void ApplyDamage(int amount)
    {
        Health.ApplyDamage(amount);
    }

    public List<InventorySlotData> GetLoot()
    {
        return LootProvider.GetLoot();
    }
}