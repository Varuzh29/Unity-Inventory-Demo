using UnityEngine;

[CreateAssetMenu(fileName = "New Game Config", menuName = "Game Config")]
public class GameConfigSO : ScriptableObject
{
    [SerializeField, Min(1)] private int _playerHP = 100, _maxPlayerHP = 100;
    [SerializeField, Min(1)] private int _enemyHP = 100, _maxEnemyHP = 100;
    [SerializeField, Min(1)] private int _inventorySlotCount = 30;
    [SerializeField, Min(0)] private int _pistolDamage = 5, _rifleDamage = 9;
    [SerializeField, Min(0)] private int _enemyDamage = 15;
    [SerializeField] private ConfigItem[] _initialPlayerItems;

    public GameConfig ToGameConfig() => new(_playerHP, _maxPlayerHP, _enemyHP, _maxEnemyHP, _inventorySlotCount, _pistolDamage, _rifleDamage, _enemyDamage, _initialPlayerItems);

    private void OnValidate() {
        foreach (var item in _initialPlayerItems)
        {
            item.Validate();
        }
    }
}