using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private InventorySlotView _slotPrefab;
    [SerializeField] private Transform _dragParent;
    private IReadOnlyInventory _Inventory;
    private int _beginDragSlotIndex = -1;
    public event Action<int, int> ItemMoved;
    private List<InventorySlotView> _slotViews = new();
    public event Action<int, Item> ItemClicked;

    public void Initialize(IReadOnlyInventory slotInventory)
    {
        _Inventory = slotInventory;
        CreateGrid();
    }

    private void CreateGrid()
    {
        for (int i = 0; i < _Inventory.Slots.Count; i++)
        {
            var slot = _Inventory.Slots[i];
            InventorySlotView slotView = Instantiate(_slotPrefab, transform);
            slotView.Initialize(slot.ToItem()?.Icon, slot.InventorySlotData.Quantity, _dragParent, i);
            slotView.StartDrag += OnSlotStartDrag;
            slotView.StopDrag += OnSlotStopDrag;
            slotView.Dropped += OnDropped;
            slotView.Clicked += OnSlotClicked;
            _slotViews.Add(slotView);
            slot.DataChanged += OnSlotDataChanged;
        }
    }

    private void OnSlotClicked(int slotIndex)
    {
        var slot = _Inventory.Slots[slotIndex];
        if (!slot.IsEmpty)
        {
            ItemClicked?.Invoke(slotIndex, slot.ToItem());
        }
    }

    private void OnSlotDataChanged(IReadOnlyInventorySlot slot)
    {
        int index = _Inventory.GetIndexOf(slot);
        InventorySlotView slotView = _slotViews[index];
        slotView.UpdateSlot(slot.ToItem()?.Icon, slot.InventorySlotData.Quantity);
    }

    private void OnDropped(int slotIndex)
    {
        if (_beginDragSlotIndex < 0) return;

        ItemMoved?.Invoke(_beginDragSlotIndex, slotIndex);
        _beginDragSlotIndex = -1;
    }

    private void OnSlotStartDrag(int slotIndex)
    {
        _beginDragSlotIndex = slotIndex;
    }

    private void OnSlotStopDrag(int slotIndex)
    {
        _beginDragSlotIndex = -1;
    }

    private void OnDestroy()
    {
        foreach (var slot in _Inventory.Slots)
        {
            int index = _Inventory.GetIndexOf(slot);
            InventorySlotView slotView = _slotViews[index];
            slotView.StartDrag -= OnSlotStartDrag;
            slotView.StopDrag -= OnSlotStopDrag;
            slotView.Dropped -= OnDropped;
            slot.DataChanged -= OnSlotDataChanged;
        }
    }
}
