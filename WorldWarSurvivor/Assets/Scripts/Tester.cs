using UnityEngine;

public class Tester : MonoBehaviour
{

    public Grid<InventoryCell> grid;
    public Grid<InventoryCell> grid1;


    public Vector2Int spawnPoint;

    public Vector2Int endPoint;

    public Material passMaterial;

    public int steps = 3;

    public GridObject gridObectPrefab;

    public GridObject currentGridObject;

    private void Start()
    {
        Create();
    }

    [ContextMenu("Create")]
    public void Create()
    {
        grid.CreateGrid();
    }


}
