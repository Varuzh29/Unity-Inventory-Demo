using UnityEngine;

public class PlayerPrefsStorageProvider : IStorageProvider
{
    private readonly string _key;

    public PlayerPrefsStorageProvider(string key)
    {
        _key = key;
    }

    public SaveData Load()
    {
        string json = PlayerPrefs.GetString(_key);
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }
        return JsonUtility.FromJson<SaveData>(json);
    }

    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(_key, json);
        PlayerPrefs.Save();
    }
}