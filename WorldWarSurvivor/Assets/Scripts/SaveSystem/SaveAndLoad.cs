using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class SaveAndLoad
{
    public static void Save<T>(string saveName, T savingObject) where T : new()
    {
        PlayerPrefs.SetString(saveName, JsonUtility.ToJson(savingObject));
    }

    public static T Load<T>(string saveName) where T : new()
    {
        if (PlayerPrefs.HasKey(saveName))
        {
            string loadedString = PlayerPrefs.GetString(saveName);
            return JsonUtility.FromJson<T>(loadedString);
        }

        return new();
    }

    public static void DeleteSave(string saveName)
    {
        PlayerPrefs.DeleteKey(saveName);
    }

    public static T LoadMapFromJson<T>(string directory) where T : new()
    {
        if (string.IsNullOrEmpty(directory) || !File.Exists(directory))
        {
            Debug.Log("Created new");
          return new();  
        } 

        string json = File.ReadAllText(directory);
        var data = JsonUtility.FromJson<T>(json);

        return data;
    }

    public static void SaveMap<T>(string directory, T data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(directory, json);
    }
}
