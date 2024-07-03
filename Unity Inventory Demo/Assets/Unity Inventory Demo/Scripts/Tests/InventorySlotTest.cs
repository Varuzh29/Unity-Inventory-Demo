using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class InventorySlotTest
{
    private ItemDatabase _itemDatabase;
    private InventorySlot _inventorySlot;
    private string _itemName;

    [SetUp]
    public void Setup()
    {
        _itemDatabase = new(new Dictionary<string, Item>(){
            {"9x18mm cartridges", new AmmoItem("9x18mm cartridges", 5, null, 100, WeaponType.Pistol, 1)},
            {"First Aid Kit", new HealingItem("First Aid Kit", 10, null, 100, 5)},
        });
        _inventorySlot = new InventorySlot(_itemDatabase, new InventorySlotData("", 0));
        _itemName = "9x18mm cartridges";
    }

    [Test]
    public void Add_ItemToEmptySlot_IncreasesQuantity()
    {
        // Arrange
        int quantityToAdd = 5;

        // Act
        _inventorySlot.Add(_itemName, quantityToAdd);

        // Assert
        Assert.AreEqual(_itemName, _inventorySlot.InventorySlotData.Name);
        Assert.AreEqual(quantityToAdd, _inventorySlot.InventorySlotData.Quantity);
    }

    [Test]
    public void Add_ExistingItem_AddsToExistingQuantity()
    {
        // Arrange
        int initialQuantity = 3;
        _inventorySlot.Add(_itemName, initialQuantity);

        // Act
        int quantityToAdd = 2;
        _inventorySlot.Add(_itemName, quantityToAdd);

        // Assert
        Assert.AreEqual(_itemName, _inventorySlot.InventorySlotData.Name);
        Assert.AreEqual(initialQuantity + quantityToAdd, _inventorySlot.InventorySlotData.Quantity);
    }

    [Test]
    public void Remove_ExistingItem_DecreasesQuantity()
    {
        // Arrange
        int initialQuantity = 10;
        _inventorySlot.Add(_itemName, initialQuantity);

        // Act
        int quantityToRemove = 5;
        _inventorySlot.Remove(_itemName, quantityToRemove);

        // Assert
        Assert.AreEqual(_itemName, _inventorySlot.InventorySlotData.Name);
        Assert.AreEqual(initialQuantity - quantityToRemove, _inventorySlot.InventorySlotData.Quantity);
    }

    [Test]
    public void Remove_ItemNotInSlot_DontDecreaseQuantity()
    {
        // Arrange
        string itemName = "First Aid Kit";
        int initialQuantity = 8;
        _inventorySlot.Add(_itemName, initialQuantity);

        // Act & 
        _inventorySlot.Remove(itemName, 3);

        //Assert
        Assert.AreEqual(initialQuantity, _inventorySlot.InventorySlotData.Quantity);
    }

    [Test]
    public void GetAvailableSpaceFor_ItemExists_ReturnsCorrectSpace()
    {
        // Arrange
        _inventorySlot.Add(_itemName, 5);
        int expected = _itemDatabase.GetItem(_itemName).StackSize - 5;

        // Act
        int availableSpace = _inventorySlot.GetAvailableSpaceFor(_itemName);

        // Assert
        Assert.AreEqual(expected, availableSpace);
    }

    [Test]
    public void GetAvailableQuantityFor_ItemInSlot_ReturnsCorrectQuantity()
    {
        // Arrange
        _inventorySlot.Add(_itemName, 4);
        int expected = 4;

        // Act
        int availableQuantity = _inventorySlot.GetAvailableQuantityFor(_itemName);

        // Assert
        Assert.AreEqual(expected, availableQuantity);
    }
}
