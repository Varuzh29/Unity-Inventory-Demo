using System;
using System.Collections.Generic;

[Serializable]
public class GameConfig
{
    public int PlayerHP { get; }
    public int MaxPlayerHP { get; }
    public int EnemyHP { get; }
    public int MaxEnemyHP { get; }
    public int InventorySlotCount { get; }
    public int PistolDamage { get; }
    public int RifleDamage { get; }
    public int EnemyDamage { get; }
    public ConfigItem[] InitialPlayerItems;

    public GameConfig(int playerHP, int maxPlayerHP, int enemyHP, int maxEnemyHP, int inventorySlotCount, int pistolDamage, int rifleDamage, int enemyDamage, ConfigItem[] initialPlayerItems)
    {
        PlayerHP = playerHP;
        MaxPlayerHP = maxPlayerHP;
        EnemyHP = enemyHP;
        MaxEnemyHP = maxEnemyHP;
        InventorySlotCount = inventorySlotCount;
        PistolDamage = pistolDamage;
        RifleDamage = rifleDamage;
        EnemyDamage = enemyDamage;
        InitialPlayerItems = initialPlayerItems;
    }
}