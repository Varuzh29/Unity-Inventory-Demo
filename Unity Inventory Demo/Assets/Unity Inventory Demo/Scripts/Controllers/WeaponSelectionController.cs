using UniRx;

public class WeaponSelectionController
{
    private Player _player;
    private WeaponSelectionView _weaponSelectionView;

    public WeaponSelectionController(Player player, WeaponSelectionView weaponSelectionView)
    {
        _player = player;
        _weaponSelectionView = weaponSelectionView;
        _weaponSelectionView.WeaponChanged += (type) => _player.SelectWeapon(type);
        _player.SelectedWeapon.Subscribe((type) => weaponSelectionView.SetSelectedWeapon(type));
    }
}