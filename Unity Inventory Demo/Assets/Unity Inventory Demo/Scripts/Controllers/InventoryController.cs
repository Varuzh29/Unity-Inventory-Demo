using System;

public class InventoryController
{
    private ItemPopUpView _itemPopUpView;
    private InventoryView _inventoryView;
    private Inventory _inventory;
    private Player _player;
    private int _selectedItemIndex = -1;


    public InventoryController(Inventory inventory, InventoryView inventoryView, ItemPopUpView itemPopUpView, Player player)
    {
        _inventory = inventory;
        _player = player;
        _inventoryView = inventoryView;
        _itemPopUpView = itemPopUpView;

        _inventoryView.ItemMoved += OnItemMoved;
        _inventoryView.ItemClicked += OnItemClicked;
    }

    private void OnItemClicked(int index, Item item)
    {
        _selectedItemIndex = index;
        _itemPopUpView.SetItem(item);
        _itemPopUpView.RemoveClicked += OnRemoveClicked;
        _itemPopUpView.ItemActionClicked += OnItemActionClicked;
        _itemPopUpView.gameObject.SetActive(true);
    }

    private void OnItemActionClicked()
    {
        Item item = _inventory.Slots[_selectedItemIndex].ToItem();

        if (item is HealingItem healingItem)
        {
            _player.Health.Heal(healingItem.HealAmount);
            _inventory.RemoveFromSlot(_selectedItemIndex, 1);
        }

        if (item is ClothingItem clothingItem)
        {
            var previouslyEquippedClothing = _player.EquipClothing(clothingItem);
            _inventory.RemoveFromSlot(_selectedItemIndex, 1);
            if (previouslyEquippedClothing != null)
            {
                _inventory.AddAt(_selectedItemIndex, previouslyEquippedClothing.Name, 1);
            }
        }

        if (item is AmmoItem ammoItem)
        {
            _inventory.FillSlot(_selectedItemIndex);
        }

        _itemPopUpView.gameObject.SetActive(false);
        _itemPopUpView.RemoveClicked -= OnRemoveClicked;
        _itemPopUpView.ItemActionClicked -= OnItemActionClicked;
        _selectedItemIndex = -1;
    }

    private void OnRemoveClicked()
    {
        _inventory.ClearSlot(_selectedItemIndex);
        _itemPopUpView.gameObject.SetActive(false);
        _itemPopUpView.RemoveClicked -= OnRemoveClicked;
        _itemPopUpView.ItemActionClicked -= OnItemActionClicked;
        _selectedItemIndex = -1;
    }

    private void OnItemMoved(int fromIndex, int toIndex)
    {
        _inventory.MoveItem(fromIndex, toIndex);
    }
}