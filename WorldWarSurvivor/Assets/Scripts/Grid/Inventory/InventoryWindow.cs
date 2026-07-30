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

    private HumanInventoryInfo _currentHumanInventory;

    private InventoryInfo _currentStorage;

    //private 


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
        _currentHumanInventory = humanInventory;

        _currentStorage = StorageInventory;

        InventorySystem.Instance.OpenHumanInventory(_currentHumanInventory, StorageInventory);
        InventorySystem.Instance.currentEquipment.InitializeItems(_currentHumanInventory.EquipmentInfo);
        window.gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        if (_currentHumanInventory != null)
        {
            _currentHumanInventory.Items = InventorySystem.Instance.GetCurrentUnitItems();
            _currentHumanInventory.EquipmentInfo = InventorySystem.Instance.GetCurrentUnitEquipmentItems();
        }

        if (_currentStorage != null)
            _currentStorage.Items = InventorySystem.Instance.GetCurrentStorageItems();

        InventorySystem.Instance.ClearGrids();
        InventorySystem.Instance.ClearEquipment();

        _currentHumanInventory.OnEndInventoryManipulation?.Invoke();

        _currentHumanInventory = null;
        _currentStorage = null;
        window.gameObject.SetActive(false);
    }
}
