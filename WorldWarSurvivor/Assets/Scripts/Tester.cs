using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{

    public Grid grid;

    public Vector2Int startPoint;

    public Vector2Int endPoint;
    
    public Material passMaterial;

    [ContextMenu("Create")]
    public void Create()
    {
        grid.CreateGrid();
    }

    [ContextMenu("SearchPass")]
    public void SearchPass()
    {
        foreach (var item in AStarPathfinding.GetPath(grid, startPoint, endPoint))
        {
            item.GetComponentInChildren<MeshRenderer>().material = passMaterial;
        }
    }

}
