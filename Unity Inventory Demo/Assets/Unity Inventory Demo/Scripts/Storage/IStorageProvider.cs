public interface IStorageProvider
{
    SaveData Load();
    void Save(SaveData data);
}