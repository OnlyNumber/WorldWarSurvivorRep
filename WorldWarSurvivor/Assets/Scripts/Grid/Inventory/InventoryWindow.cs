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

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;

        CloseButton.onClick.AddListener(CloseWindow);
    }

    public void OpenWindow()
    {
        InventorySystem.Instance.SpawnUnitItems((CellSelecter.Instance.CurrentObject as Human).items);
        InventorySystem.Instance.currentEquipment.InitializeItems((CellSelecter.Instance.CurrentObject as Human).equipmentInfo);
        window.gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        (CellSelecter.Instance.CurrentObject as Human).items = InventorySystem.Instance.GetCurrentUnitItems();
        (CellSelecter.Instance.CurrentObject as Human).equipmentInfo = InventorySystem.Instance.GetCurrentUnitEquipmentItems();
        InventorySystem.Instance.ClearGrids();
        InventorySystem.Instance.ClearEquipment();

        window.gameObject.SetActive(false);
    }
}
