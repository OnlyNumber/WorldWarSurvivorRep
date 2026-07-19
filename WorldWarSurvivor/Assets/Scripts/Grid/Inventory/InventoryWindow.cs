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

    private InventoryInfo CurrentStorageWindow;


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

        InventorySystem.Instance.OpenHumanInventory(CurrentHuman);
        InventorySystem.Instance.currentEquipment.InitializeItems(CurrentHuman.EquipmentInfo);
        window.gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        CurrentHuman.Items = InventorySystem.Instance.GetCurrentUnitItems();
        CurrentHuman.EquipmentInfo = InventorySystem.Instance.GetCurrentUnitEquipmentItems();
        InventorySystem.Instance.ClearGrids();
        InventorySystem.Instance.ClearEquipment();


        CurrentHuman = null;
        window.gameObject.SetActive(false);
    }
}
