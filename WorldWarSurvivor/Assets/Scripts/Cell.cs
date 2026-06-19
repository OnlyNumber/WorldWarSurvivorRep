using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int Coordinate;

    public bool IsCellNoPath;

    public void Initialize(Vector2Int coordintes)
    {
        Coordinate = coordintes;
    }
}
