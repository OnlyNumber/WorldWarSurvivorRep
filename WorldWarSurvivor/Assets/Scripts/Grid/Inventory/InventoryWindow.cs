using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindow : MonoBehaviour
{
    public static InventoryWindow Instance;

    [SerializeField] private Image window;

    public Button CloseButton;

    public RectTransform ItemsTransform;

    private HumanInventoryInfo CurrentHuman;

    private InventoryInfo CurrentStorage;


    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;

        CloseButton.onClick.AddListener(CloseWindow);
    }

    public void OpenWindow(HumanInventoryInfo humanInventory, InventoryInfo StorageInventory = null)
    {
        CurrentHuman = humanInventory;

        CurrentStorage = StorageInventory;

        InventorySystem.Instance.OpenHumanInventory(CurrentHuman, StorageInventory);
        InventorySystem.Instance.currentEquipment.InitializeItems(CurrentHuman.EquipmentInfo);
        window.gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        if (CurrentHuman != null)
        {
            CurrentHuman.Items = InventorySystem.Instance.GetCurrentUnitItems();
            CurrentHuman.EquipmentInfo = InventorySystem.Instance.GetCurrentUnitEquipmentItems();
        }

        if (CurrentStorage != null)
            CurrentStorage.Items = InventorySystem.Instance.GetCurrentStorageItems();

        InventorySystem.Instance.ClearGrids();
        InventorySystem.Instance.ClearEquipment();


        CurrentHuman = null;
        CurrentStorage = null;
        window.gameObject.SetActive(false);
    }
}
