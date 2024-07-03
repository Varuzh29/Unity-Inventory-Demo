using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopUpView : MonoBehaviour
{
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemWeightText;
    [SerializeField] private TMP_Text itemPropertyText;
    [SerializeField] private Image itemPropertyImage;
    [SerializeField] private GameObject itemPropertyGO;
    [SerializeField] private Sprite protectionSprite;
    [SerializeField] private Sprite healSprite;
    [SerializeField] private Button itemActionButton;
    [SerializeField] private TMP_Text itemActionButtonText;
    [SerializeField] private Button removeButton;
    [SerializeField] private Button backgroundButton;
    public event Action RemoveClicked;
    public event Action ItemActionClicked;


    private void OnEnable()
    {
        backgroundButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

        removeButton.onClick.AddListener(() =>
        {
            RemoveClicked?.Invoke();
            gameObject.SetActive(false);
        });
    }

    private void OnDisable()
    {
        backgroundButton.onClick.RemoveAllListeners();
        removeButton.onClick.RemoveAllListeners();
        itemActionButton.onClick.RemoveAllListeners();
        RemoveClicked = null;
        ItemActionClicked = null;
    }

    public void SetItem(Item item)
    {
        headerText.text = item.Name;
        itemImage.sprite = item.Icon;
        itemWeightText.text = $"{item.Weight} kg";
        itemPropertyGO.SetActive(true);
        itemActionButton.gameObject.SetActive(true);
        itemActionButton.onClick.AddListener(() =>
        {
            ItemActionClicked?.Invoke();
            gameObject.SetActive(false);
        });
        switch (item)
        {
            case ClothingItem clothingItem:
                itemPropertyText.text = $"+{clothingItem.Protection}";
                itemPropertyImage.sprite = protectionSprite;
                itemActionButtonText.text = "Equip";
                break;
            case HealingItem healingItem:
                itemPropertyText.text = $"+{healingItem.HealAmount}";
                itemPropertyImage.sprite = healSprite;
                itemActionButtonText.text = "Use";
                break;
            case AmmoItem ammoItem:
                itemPropertyGO.SetActive(false);
                itemActionButtonText.text = "Buy";
                break;
            default:
                itemPropertyGO.SetActive(false);
                itemActionButton.gameObject.SetActive(false);
                break;
        }
    }
}
