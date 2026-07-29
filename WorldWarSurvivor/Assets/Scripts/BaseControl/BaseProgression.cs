using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseProgression : MonoBehaviour
{
    public const string PlayerSavePath = "PlayerData";

    public static BaseProgression Instance;
    public BaseProgressionData PlayerData;

    public int CurrentCommandPrice;

    [field: SerializeField]
    public int MaxCommandPrice
    {
        get;
        private set;
    }


    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        LoadInfo();
    }



    private void LoadInfo()
    {
        string folderPath = Path.Combine(Application.dataPath + "/" + "PlayerSave", "PlayerDataSave" + ".json");

        
        PlayerData = SaveAndLoad.LoadMapFromJson<BaseProgressionData>(folderPath);
        PlayerData.GetMyItemFromIndex();
    }


    public void SaveInfo()
    {
        string folderPath = Path.Combine(Application.dataPath + "/" + "PlayerSave", "PlayerDataSave" + ".json");
        
        PlayerData.SetMyItemIndex();
        SaveAndLoad.SaveMap(folderPath, PlayerData);



    }

    public void DeleteInfo()
    {
        SaveAndLoad.DeleteSave(PlayerSavePath);
    }
}
