using UniRx;
using UnityEngine;

public class SaveSystem
{
    private IStorageProvider _storageProvider;
    private SaveData _saveData;
    private Inventory _inventory;
    private Player _player;
    private Enemy _enemy;
    private GameConfig _gameConfig;

    public SaveSystem(IStorageProvider storageProvider, GameConfig gameConfig, Inventory inventory, Player player, Enemy enemy, ItemDatabase itemDatabase)
    {
        _gameConfig = gameConfig;
        _player = player;
        _enemy = enemy;
        _inventory = inventory;
        _storageProvider = storageProvider;
        _saveData = _storageProvider.Load();
        _saveData ??= new()
        {
            PlayerHP = gameConfig.PlayerHP,
            EnemyHP = gameConfig.EnemyHP
        };

        _player.Health.HP.Value = _saveData.PlayerHP;
        if (!string.IsNullOrWhiteSpace(_saveData.PlayerHeadClothing))
        {
            _player.EquipClothing((ClothingItem)itemDatabase.GetItem(_saveData.PlayerHeadClothing));
        }
        if (!string.IsNullOrWhiteSpace(_saveData.PlayerTorsoClothing))
        {
            Debug.Log(_saveData.PlayerTorsoClothing);
            _player.EquipClothing((ClothingItem)itemDatabase.GetItem(_saveData.PlayerTorsoClothing));
        }
        _player.SelectWeapon((WeaponType)_saveData.PlayerSelectedWeapon);

        _enemy.Health.HP.Value = _saveData.EnemyHP;

        if (_saveData.inventorySlotsData == null)
        {
            AddInitialItems();
        }
        else
        {
            _inventory.SetInventoryData(_saveData.inventorySlotsData);
        }

        _player.SelectedWeapon.Subscribe(_ => SaveGame());
        _player.Health.HP.Subscribe(_ => SaveGame());
        _player.HeadClothing.Subscribe(_ => SaveGame());
        _player.TorsoClothing.Subscribe(_ => SaveGame());
        _enemy.Health.HP.Subscribe(_ => SaveGame());
        _inventory.DataChanged += () => SaveGame();
    }

    public void AddInitialItems()
    {
        foreach (var configItem in _gameConfig.InitialPlayerItems)
        {
            _inventory.Add(configItem.item.name, configItem.quantity);
        }
    }

    public void SaveGame()
    {
        _saveData.inventorySlotsData = _inventory.GetInventoryData();
        _saveData.PlayerHeadClothing = _player.HeadClothing.Value?.Name;
        _saveData.PlayerTorsoClothing = _player.TorsoClothing.Value?.Name;
        _saveData.PlayerSelectedWeapon = (int)_player.SelectedWeapon.Value;
        _saveData.PlayerHP = _player.Health.HP.Value;
        _saveData.EnemyHP = _enemy.Health.HP.Value;
        _storageProvider.Save(_saveData);
    }
}