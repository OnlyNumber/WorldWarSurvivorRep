using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    #region Containers

    //public List<InventoryGrid> inventoryGrids = new();
    public EquipmentController currentEquipment;
    [SerializeField] private InventoryGrid unitInventoryGrid;
    [SerializeField] private InventoryGrid storageInventoryGrid;

    #endregion

    #region  Current Item
    public InventoryItem currentItem;

    private InventoryGrid _lastGrid;
    private Vector3 _lastPlacePosition;
    private Direciton _lastDireciton;

    #endregion

    #region  Marks
    [SerializeField] private Image markPrefab;

    private List<Image> markedCells = new();

    [SerializeField] private Color NotPlaceable;
    [SerializeField] private Color Placeable;
    [SerializeField] private Color NotDisturb;
    #endregion

    [SerializeField] private InventoryItem emptyItemPrefab;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //unitInventoryGrid.CreateGrid();
    }

    private void Update()
    {
        if (currentItem == null)
            return;

        MarkPlacementPositions();
    }

    public void OpenHumanInventory(InventoryInfo humanInventoryInfo, InventoryInfo storageInventory = null)
    {
        if (humanInventoryInfo != null)
        {
            CreateGrid(unitInventoryGrid, humanInventoryInfo);
            SpawnItems(unitInventoryGrid, humanInventoryInfo.Items);
        }

        if (storageInventory != null)
        {
            CreateGrid(storageInventoryGrid, storageInventory);
            SpawnItems(storageInventoryGrid, storageInventory.Items);
        }


        void CreateGrid(InventoryGrid inventoryGrid, InventoryInfo humanInventoryInfo)
        {
            inventoryGrid.CreateGrid(humanInventoryInfo.Size.x, humanInventoryInfo.Size.y);
        }

        void SpawnItems(InventoryGrid inventoryGrid, List<InventoryItemInfo> spawnItems)
        {
            foreach (var item in spawnItems)
            {
                if (!item.IsItemExist)
                    continue;

                var emptyItem = SpawnItem(item);

                emptyItem.SetPositionReferencedByCell(inventoryGrid.GetCell(item.FirstCellPosition).MyRectTransform.position);
                inventoryGrid.TyrPlaceItem(emptyItem, emptyItem.grabbingItem.MyRectTransform.position);
            }
        }
    }

    public InventoryItem SpawnItem(InventoryItemInfo unitItemInfo)
    {
        if (unitItemInfo == null || !unitItemInfo.IsItemExist)
            return null;

        var emptyItem = Instantiate(emptyItemPrefab);
        emptyItem.Initialize(unitItemInfo);

        emptyItem.transform.SetParent(InventoryWindow.Instance.ItemsTransform);
        if (unitItemInfo.direciton == Direciton.Up)
            emptyItem.transform.rotation = Quaternion.Euler(0, 0, 90);

        return emptyItem;
    }


    public void ClearGrids()
    {
        List<InventoryGrid> inventoryGrids = new();

        inventoryGrids.Add(unitInventoryGrid);
        inventoryGrids.Add(storageInventoryGrid);


        foreach (var item in inventoryGrids)
            item.ClearGrid();

        foreach (var item in inventoryGrids)
            item.ClearCells();
    }

    public void ClearEquipment()
    {
        currentEquipment.ClearEquipment();
    }

    public List<InventoryItemInfo> GetCurrentUnitItems() => unitInventoryGrid.GetItemsInfo();
    public EquipmentInfo GetCurrentUnitEquipmentItems() => currentEquipment.GetItems();

    public List<InventoryItemInfo> GetCurrentStorageItems() => storageInventoryGrid.GetItemsInfo();


    public void PickUpItem(InventoryItem inventoryItem)
    {
        currentItem = inventoryItem;
        _lastDireciton = currentItem.info.direciton;
        _lastPlacePosition = inventoryItem.grabbingItem.MyRectTransform.position;
        _lastGrid = null;

        CreateMarkingCells();

        List<InventoryGrid> inventoryGrids = new();

        inventoryGrids.Add(unitInventoryGrid);
        inventoryGrids.Add(storageInventoryGrid);

        foreach (var item in inventoryGrids)
        {
            if (item.InventoryItems.Contains(inventoryItem))
            {
                _lastGrid = item;
                _lastGrid.RemoveItem(inventoryItem);
                return;
            }
        }

        currentEquipment.RemoveItem(inventoryItem);



    }

    public void DropItem(InventoryItem inventoryItem)
    {
        InventoryGrid gridForPlace = null;
        List<InventoryGrid> inventoryGrids = new();

        inventoryGrids.Add(unitInventoryGrid);
        inventoryGrids.Add(storageInventoryGrid);

        foreach (var item in inventoryGrids)
        {
            if (item.GetCellFromPosition(Input.mousePosition) != null)
            {
                gridForPlace = item;
                break;
            }
        }
        var itemPosition = inventoryItem.grabbingItem.MyRectTransform.position;

        if ((gridForPlace == null || !gridForPlace.TyrPlaceItem(inventoryItem, itemPosition)) &&
         !currentEquipment.TryPlaceItem(inventoryItem, itemPosition))
        {
            inventoryItem.info.direciton = _lastDireciton;

            if (inventoryItem.info.direciton == Direciton.Up)
                inventoryItem.grabbingItem.MyRectTransform.rotation = Quaternion.Euler(0, 0, 90);
            else
                inventoryItem.grabbingItem.MyRectTransform.rotation = Quaternion.Euler(0, 0, 0);


            if (_lastGrid != null)
                _lastGrid.TyrPlaceItem(inventoryItem, _lastPlacePosition);
            else
                currentEquipment.TryPlaceItem(inventoryItem, _lastPlacePosition);

        }


        ClearMarkingCells();
        currentItem = null;
    }

    private void CreateMarkingCells()
    {
        int count = currentItem.info.Size.x * currentItem.info.Size.y;

        for (int i = 0; i < count; i++)
        {
            markedCells.Add(Instantiate(markPrefab, currentItem.grabbingItem.MyRectTransform));
        }
    }

    private void ClearMarkingCells()
    {
        for (int i = 0; i < markedCells.Count; i++)
        {
            Destroy(markedCells[i].gameObject);
        }

        markedCells.Clear();
    }

    private void MarkPlacementPositions()
    {
        var positions = currentItem.GetItemPlacePositions(currentItem.grabbingItem.MyRectTransform.position);

        for (int i = 0; i < markedCells.Count; i++)
        {
            var cell = CheckPlacementPosition(positions[i]);
            markedCells[i].color = NotDisturb;

            if (cell == null)
                continue;

            if (cell.IsOccupied)
                markedCells[i].color = NotPlaceable;
            else
                markedCells[i].color = Placeable;

            markedCells[i].rectTransform.position = cell.MyRectTransform.position;
        }
    }

    public InventoryCell CheckPlacementPosition(Vector2 positionOfCheck)
    {
        InventoryGrid gridForPlace = null;
        List<InventoryGrid> inventoryGrids = new();

        inventoryGrids.Add(unitInventoryGrid);
        inventoryGrids.Add(storageInventoryGrid);

        foreach (var item in inventoryGrids)
        {
            if (item.GetCellFromPosition(Input.mousePosition) != null)
            {
                gridForPlace = item;
                break;
            }
        }

        if (gridForPlace != null)
            return gridForPlace.GetCellFromPosition(positionOfCheck);

        return null;

    }

}
