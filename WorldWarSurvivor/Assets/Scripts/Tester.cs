using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tester : MonoBehaviour
{

    public Grid grid;

    public Vector2Int spawnPoint;

    public Vector2Int endPoint;

    public Material passMaterial;

    public int steps = 3;

    public GridObject gridObectPrefab;

    public GridObject currentGridObject;

    private void Start()
    {
        Create();

        SpawnGridObject();
    }

    [ContextMenu("Create")]
    public void Create()
    {
        grid.CreateGrid();
    }

    [ContextMenu("SpawnGridObject")]
    public void SpawnGridObject()
    {
        //currentGridObject = Instantiate(gridObectPrefab);
        grid.SpawnGridObject(spawnPoint, gridObectPrefab);
    }

}
