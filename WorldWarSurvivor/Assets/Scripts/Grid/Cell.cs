using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int Coordinate;

    public void Initialize(Vector2Int coordintes)
    {
        Coordinate = coordintes;
    }
}
