using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

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
        PlayerData = SaveAndLoad.Load<BaseProgressionData>(PlayerSavePath);
    }

    public void SaveInfo()
    {
        Debug.Log("Save info");
        SaveAndLoad.Save(PlayerSavePath, PlayerData);
    }

    public void DeleteInfo()
    {
        SaveAndLoad.DeleteSave(PlayerSavePath);
    }
}
