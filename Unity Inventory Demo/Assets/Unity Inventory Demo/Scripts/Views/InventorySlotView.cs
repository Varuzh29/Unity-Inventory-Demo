using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniRx;
using System;

public class InventorySlotView : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TMP_Text _quantityText;
    private Transform _dragParent;
    public event Action<int> StartDrag;
    public event Action<int> StopDrag;
    public event Action<int> Dropped;
    public event Action<int> Clicked;
    private int _index;
    private int _imageSibilingIndex;

    public void Initialize(Sprite itemImage, int quantity, Transform dragParent, int index)
    {
        _index = index;
        _dragParent = dragParent;
        _imageSibilingIndex = _itemImage.transform.GetSiblingIndex();
        UpdateSlot(itemImage, quantity);
    }

    public void UpdateSlot(Sprite itemImage, int quantity)
    {
        if (itemImage == null)
        {
            _itemImage.gameObject.SetActive(false);
        }
        else
        {
            _itemImage.sprite = itemImage;
            _itemImage.gameObject.SetActive(true);
        }
        _quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _itemImage.transform.SetParent(_dragParent);
        _itemImage.transform.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _itemImage.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        StartDrag?.Invoke(_index);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _itemImage.transform.SetParent(transform);
        _itemImage.transform.SetSiblingIndex(_imageSibilingIndex);
        _itemImage.transform.localScale = new Vector3(1f, 1f, 1f);
        _itemImage.transform.localPosition = Vector3.zero;
        StopDrag?.Invoke(_index);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Dropped?.Invoke(_index);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked?.Invoke(_index);
    }
}
