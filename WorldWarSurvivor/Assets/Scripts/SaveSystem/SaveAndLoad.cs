using System.Collections;
using System.Collections.Generic;
using System.IO;
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
}
