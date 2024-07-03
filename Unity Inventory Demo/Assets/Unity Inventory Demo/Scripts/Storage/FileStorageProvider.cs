using UnityEngine;
using System.IO;

public class FileStorageProvider : IStorageProvider
{
    private readonly string _key;

    public FileStorageProvider(string key)
    {
        _key = key;
    }

    public string GetFilePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    public SaveData Load()
    {
        string path = GetFilePath(_key);

        if (!File.Exists(path))
        {
            return null;
        }

        using StreamReader file = new(path);
        string json = file.ReadToEnd();
        return JsonUtility.FromJson<SaveData>(json);
    }

    public void Save(SaveData data)
    {
        string path = GetFilePath(_key);
        string json = JsonUtility.ToJson(data);
        using StreamWriter file = new(path);
        file.Write(json);
    }
}