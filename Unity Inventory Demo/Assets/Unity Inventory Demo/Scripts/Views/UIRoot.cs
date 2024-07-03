using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIRoot : MonoBehaviour
{
    [SerializeField] private Button _shotButton;
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private HealthBarView _playerHealthBarView;
    [SerializeField] private HealthBarView _enemyHealthBarView;
    [SerializeField] private ItemPopUpView _itemPopUpView;
    [SerializeField] private GameObject _gameOverPopUp;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Image headClothingImage;
    [SerializeField] private Image torsoClothingImage;
    [SerializeField] private TMP_Text headProtectionText;
    [SerializeField] private TMP_Text torsoProtectionText;
    [SerializeField] private WeaponSelectionView _weaponSelectionView;
    public event Action ShootClicked;
    public event Action RetryClicked;

    public void Initialize(
        IReadOnlyReactiveProperty<int> enemyHealthProperty,
        IReadOnlyReactiveProperty<int> enemyMaxHPProperty,
        Inventory inventory,
        Player player
        )
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        WeaponSelectionController weaponSelectionController = new(player, _weaponSelectionView);

        player.Health.MaxHP.Subscribe(_playerHealthBarView.SetMaxHP);
        player.Health.HP.Subscribe(_playerHealthBarView.SetHP);
        enemyMaxHPProperty.Subscribe((enemyMaxHP) =>
        {
            _enemyHealthBarView.SetMaxHP(enemyMaxHP);
        });
        enemyHealthProperty.Subscribe((enemyHealth) =>
        {
            _enemyHealthBarView.SetHP(enemyHealth);
        });

        player.HeadClothing.Subscribe((playerHeadProtection) =>
        {
            headClothingImage.sprite = playerHeadProtection?.Icon;
            headClothingImage.gameObject.SetActive(playerHeadProtection != null);
            headProtectionText.text = playerHeadProtection != null ? playerHeadProtection.Protection.ToString() : "0";
        });
        player.TorsoClothing.Subscribe((playerTorsoProtection) =>
        {
            torsoClothingImage.sprite = playerTorsoProtection?.Icon;
            torsoClothingImage.gameObject.SetActive(playerTorsoProtection != null);
            torsoProtectionText.text = playerTorsoProtection != null ? playerTorsoProtection.Protection.ToString() : "0";
        });

        _shotButton.onClick.AddListener(() =>
        {
            ShootClicked?.Invoke();
        });

        _retryButton.onClick.AddListener(() =>
        {
            _gameOverPopUp.SetActive(false);
            RetryClicked?.Invoke();
        });

        _inventoryView.Initialize(inventory);
        InventoryController inventoryController = new(inventory, _inventoryView, _itemPopUpView, player);
    }

    public void ShowLoadingScreen()
    {
        _loadingScreen.SetActive(true);
    }

    public void ShowGameOverPopUp()
    {
        _gameOverPopUp.SetActive(true);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _loadingScreen.SetActive(false);
    }
}