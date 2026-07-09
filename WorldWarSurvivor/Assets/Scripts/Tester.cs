using UnityEngine;

public class Tester : MonoBehaviour
{

    public BoardGrid grid;

    public Vector2Int spawnPoint;

    public Vector2Int endPoint;

    [ContextMenu("FindPath")]
    public void FindPath()
    {
        var path = AStarPathfinding.FindPath(grid, spawnPoint, endPoint, true);

        if(path == null)
        Debug.Log("what");

        path.Remove(path[0]);

        foreach (var item in path)
        {
            Debug.Log("Coordinate " + item.Coordinate);
        }
    }


}
