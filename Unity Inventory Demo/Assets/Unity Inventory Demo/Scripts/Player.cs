using UniRx;

public class Player
{
    public ReactiveProperty<WeaponType> SelectedWeapon { get; } = new();
    public Health Health { get; }
    public ReactiveProperty<ClothingItem> HeadClothing = new();
    public ReactiveProperty<ClothingItem> TorsoClothing = new();

    public Player(int hp, int maxHp)
    {
        Health = new Health(hp, maxHp);
        SelectedWeapon.Value = WeaponType.Pistol;
    }

    public void SelectWeapon(WeaponType weapon)
    {
        SelectedWeapon.Value = weapon;
    }

    public void ApplyDamage(int amount, DamageDirection direction)
    {
        switch (direction)
        {
            case DamageDirection.Head:
                amount -= HeadClothing.Value != null ? HeadClothing.Value.Protection : 0;
                break;

            case DamageDirection.Torso:
                amount -= TorsoClothing.Value != null ? TorsoClothing.Value.Protection : 0;
                break;
        }
        Health.ApplyDamage(amount);
    }

    public ClothingItem EquipClothing(ClothingItem clothing)
    {
        ClothingItem previouslyEquippedClothing = null;

        if (clothing.Type == ClothingType.Head)
        {
            previouslyEquippedClothing = HeadClothing.Value;
            HeadClothing.Value = clothing;
        }

        if (clothing.Type == ClothingType.Torso)
        {
            previouslyEquippedClothing = TorsoClothing.Value;
            TorsoClothing.Value = clothing;
        }

        return previouslyEquippedClothing;
    }
}