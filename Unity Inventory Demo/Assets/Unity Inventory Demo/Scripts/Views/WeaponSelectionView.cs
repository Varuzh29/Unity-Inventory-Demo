using System;
using UnityEngine;

public class WeaponSelectionView : MonoBehaviour
{
    [SerializeField] private SelectableButton pistolButton;
    [SerializeField] private SelectableButton riffleButton;
    public event Action<WeaponType> WeaponChanged;
    private WeaponType selectedWeapon;

    private void OnEnable()
    {
        pistolButton.OnClick += () => WeaponSelected(WeaponType.Pistol);
        riffleButton.OnClick += () => WeaponSelected(WeaponType.Riffle);
    }

    private void WeaponSelected(WeaponType weaponType)
    {
        if (selectedWeapon == weaponType) return;
        selectedWeapon = weaponType;
        WeaponChanged?.Invoke(weaponType);
    }

    public void SetSelectedWeapon(WeaponType weaponType)
    {
        selectedWeapon = weaponType;
        pistolButton.Selected = selectedWeapon == WeaponType.Pistol;
        riffleButton.Selected = selectedWeapon == WeaponType.Riffle;
    }
}
