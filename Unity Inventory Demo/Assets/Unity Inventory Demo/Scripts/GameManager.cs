using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class GameManager
{
    [SerializeField] private GameConfigSO _gameConfigSO;
    [SerializeField] private UIRoot _uIRootPrefab;
    private UIRoot _uIRoot;
    private Player _player;
    private Enemy _enemy;
    private Inventory _inventory;
    private GameConfig _gameConfig;
    private SaveSystem _saveSystem;

    public GameManager(GameConfig gameConfig, IStorageProvider storageProvider, ItemDatabase itemDatabase, UIRoot uIRoot, ILootProvider lootProvider)
    {
        _gameConfig = gameConfig;
        _inventory = new(itemDatabase, _gameConfig.InventorySlotCount);
        _player = new(_gameConfig.PlayerHP, _gameConfig.MaxPlayerHP);
        _enemy = new(_gameConfig.EnemyHP, _gameConfig.MaxEnemyHP, lootProvider);
        _saveSystem = new(storageProvider, _gameConfig, _inventory, _player, _enemy, itemDatabase);
        _uIRoot = uIRoot;
        _uIRoot.Initialize(_enemy.Health.HP, _enemy.Health.MaxHP, _inventory, _player);
        _uIRoot.RetryClicked += () =>
        {
            RestartGame();
        };
        _uIRoot.ShootClicked += () =>
        {
            TryShoot();
        };
        _player.Health.IsAlive.Subscribe((isAlive) =>
        {
            if (isAlive == false) GameOver();
        });
        _enemy.Health.IsAlive.Subscribe((isAlive) =>
        {
            if (isAlive == false) GiveLootToPlayer();
        });
        LoadScene(Scenes.Game);
    }

    private void LoadScene(Scenes scene)
    {
        _uIRoot.ShowLoadingScreen();
        SceneManager.LoadScene((int)scene);
    }

    private void GameOver()
    {
        _uIRoot.ShowGameOverPopUp();
        RestartGame();
    }

    private void GiveLootToPlayer()
    {
        var loot = _enemy.GetLoot();
        foreach (var item in loot)
        {
            _inventory.Add(item.Name, item.Quantity);
        }
        _enemy.Health.Reanimate();
    }

    private void RestartGame()
    {
        _player.Health.Reanimate();
        _enemy.Health.Reanimate();
        _player.SelectWeapon(WeaponType.Pistol);
        _inventory.ClearAll();
        _player.HeadClothing.Value = null;
        _player.TorsoClothing.Value = null;
        _saveSystem.AddInitialItems();
        _saveSystem.SaveGame();
    }

    private bool TryShoot()
    {
        WeaponType weaponType = _player.SelectedWeapon.Value;

        foreach (var slot in _inventory.Slots)
        {
            var item = slot.ToItem();
            if (item == null) continue;
            if (item is AmmoItem ammoItem)
            {
                if (_player.SelectedWeapon.Value == ammoItem.Type)
                {
                    int quantity = slot.InventorySlotData.Quantity;
                    int consumption = ammoItem.Сonsumption;
                    if (quantity < consumption) return false;
                    _inventory.Remove(item.Name, consumption);
                    int damage = 0;
                    switch (_player.SelectedWeapon.Value)
                    {
                        case WeaponType.Pistol:
                            damage = _gameConfig.PistolDamage;
                            break;

                        case WeaponType.Riffle:
                            damage = _gameConfig.RifleDamage;
                            break;
                    }
                    _enemy.ApplyDamage(damage);
                    _player.ApplyDamage(_gameConfig.EnemyDamage, _enemy.GetDamageDirection());
                    return true;
                }
            }
        }
        return false;
    }
}