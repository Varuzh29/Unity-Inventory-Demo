using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameConfigSO _gameConfigSO;
    [SerializeField] private UIRoot _uIRootPrefab;

    private void Start()
    {
        GameConfig gameConfig = _gameConfigSO.ToGameConfig();
        IStorageProvider storageProvider = new FileStorageProvider("Save 18");
        IItemsDataProvider itemsDataProvider = new AddressablesItemsDataProvider();
        itemsDataProvider.LoadItems(itemsData =>
        {
            ItemDatabase itemDatabase = new(itemsData);
            UIRoot uIRoot = Instantiate(_uIRootPrefab);
            uIRoot.name = "UI Root";
            DontDestroyOnLoad(uIRoot.gameObject);
            ILootProvider lootProvider = new RandomLootProvider(itemDatabase);
            GameManager gameManager = new(gameConfig, storageProvider, itemDatabase, uIRoot, lootProvider);
        });
    }
}