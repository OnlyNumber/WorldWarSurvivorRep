using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    [SerializeField] private BoardGrid boardGrid;
    [SerializeField] private Transform mapParent;

    private List<GridObject> allCreatedObjects;

    private List<GameObject> _availablePrefabs = new List<GameObject>();

    private Vector3 cellSize = new Vector3(1f, 1f, 1f);



    public void Create(string pathOfMap)
    {
        string json = File.ReadAllText(GetMapDataDirectory() + "/" + pathOfMap + ".json");
        MapData mapData = JsonUtility.FromJson<MapData>(json);

        foreach (var item in mapData.placedObjects)
        {
            var data = item;

            GameObject prefab = _availablePrefabs.Find(p => p != null && p.name == data.prefabName);

            if (prefab == null && !string.IsNullOrEmpty(data.prefabPath))
            {
                prefab = AssetDatabase.LoadAssetAtPath<GameObject>(data.prefabPath);

                if (prefab != null && !_availablePrefabs.Contains(prefab))
                {
                    _availablePrefabs.Add(prefab);
                }
            }

            if (prefab != null)
            {
                Vector3 spawnPos = GridToWorldPosition(data.gridPosition);

                Vector2Int coordinate = CalculactePosition(spawnPos, data.direction);
                var currentObj = PrefabUtility.InstantiatePrefab(prefab).GetComponent<GridObject>();

                if (currentObj is MultipleCellGridObject multipleObj)
                    multipleObj.myDirection = data.direction;


                boardGrid.SpawnGridObject(coordinate, currentObj.GetComponent<GridObject>(), true);

                currentObj.transform.parent = mapParent;
                allCreatedObjects.Add(currentObj);

            }
        }
    }



    private Vector2Int CalculactePosition(Vector3 position, Direction direction)
    {
        return boardGrid.GetCellFromWorldPosition(position + (Vector3)Utilities.DirectionToPosition(direction) * 0.5f).Coordinate;
    }


    private void AddObjectToGrid(GridObject gridObject)
    {

    }



    private string GetMapDataDirectory()
    {
        string folderPath = Path.Combine(Application.dataPath, "MapData");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            AssetDatabase.Refresh();
        }
        return folderPath;
    }

    private Vector3 GridToWorldPosition(Vector3Int gridPos)
    {
        return new Vector3(
            gridPos.x * cellSize.x,
            gridPos.y * cellSize.y,
            gridPos.z * cellSize.z
        );
    }
}
