using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    private List<GameObject> availablePrefabs = new List<GameObject>();
    private int selectedPrefabIndex = 0;
    
    private Vector3 cellSize = new Vector3(1f, 1f, 1f);
    private Transform mapParent;

    // Словник тепер зберігає СПИСОК об'єктів для кожної клітинки
    private Dictionary<Vector3Int, List<GameObject>> spawnedObjects = new Dictionary<Vector3Int, List<GameObject>>();

    private GameObject previewObject;
    private Vector3Int currentDragGridPos;
    private bool isDragging = false;
    private Direction currentDirection = Direction.Up;

    [MenuItem("Tools/Map Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("Map Editor");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        CancelDragging();
    }

    private void OnGUI()
    {
        GUILayout.Label("Налаштування Редактора Мапи", EditorStyles.boldLabel);

        cellSize = EditorGUILayout.Vector3Field("Розмір клітинки (Cell Size)", cellSize);
        mapParent = (Transform)EditorGUILayout.ObjectField("Батьківський Transform", mapParent, typeof(Transform), true);

        EditorGUILayout.Space(10);
        GUILayout.Label("Палітра Префабів (Мешів)", EditorStyles.boldLabel);

        int newCount = EditorGUILayout.IntField("Кількість префабів", availablePrefabs.Count);
        newCount = Mathf.Max(0, newCount);

        while (availablePrefabs.Count < newCount) availablePrefabs.Add(null);
        while (availablePrefabs.Count > newCount) availablePrefabs.RemoveAt(availablePrefabs.Count - 1);

        EditorGUI.indentLevel++;
        for (int i = 0; i < availablePrefabs.Count; i++)
        {
            availablePrefabs[i] = (GameObject)EditorGUILayout.ObjectField(
                $"Префаб #{i + 1}", 
                availablePrefabs[i], 
                typeof(GameObject), 
                false
            );
        }
        EditorGUI.indentLevel--;

        if (availablePrefabs.Count > 0)
        {
            EditorGUILayout.Space(5);
            
            if (selectedPrefabIndex >= availablePrefabs.Count)
                selectedPrefabIndex = 0;

            string[] names = new string[availablePrefabs.Count];
            for (int i = 0; i < availablePrefabs.Count; i++)
            {
                names[i] = availablePrefabs[i] != null ? availablePrefabs[i].name : $"[Порожньо #{i + 1}]";
            }
            
            selectedPrefabIndex = EditorGUILayout.Popup("Активний префаб для малювання", selectedPrefabIndex, names);
        }

        EditorGUILayout.Space(15);
        
        if (GUILayout.Button("Зберегти мапу в JSON", GUILayout.Height(30)))
        {
            SaveMapToJson();
        }

        if (GUILayout.Button("Завантажити мапу з JSON", GUILayout.Height(30)))
        {
            LoadMapFromJson();
        }

        if (GUILayout.Button("Очистити мапу на сцені", GUILayout.Height(25)))
        {
            ClearSceneMap();
        }

        EditorGUILayout.HelpBox("ІНСТРУКЦІЯ:\n" +
                               "• Shift + ЛКМ + Перетягування — Поставити префаб (підтримується декілька об'єктів на клітинку)\n" +
                               "• R або E (під час перетягування) — Поворот за часовою стрілкою (Up -> Right -> Down -> Left)\n" +
                               "• Q (під час перетягування) — Поворот проти часової стрілки\n" +
                               "• Відпустити ЛКМ — Закріпити об'єкт\n" +
                               "• Shift + ПКМ — Видалити останній доданий об'єкт на клітинці", MessageType.Info);
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        Plane hPlane = new Plane(Vector3.up, Vector3.zero);

        if (!hPlane.Raycast(ray, out float distance)) return;

        Vector3 worldPoint = ray.GetPoint(distance);
        Vector3Int gridPos = WorldToGridPosition(worldPoint);

        // Обробка гарячих клавіш для зміни Direction
        if (isDragging && previewObject != null && e.type == EventType.KeyDown)
        {
            if (e.keyCode == KeyCode.R || e.keyCode == KeyCode.E)
            {
                currentDirection = RotateClockwise(currentDirection);
                previewObject.transform.rotation = Utilities.DirectionToRotation(currentDirection);
                e.Use();
            }
            else if (e.keyCode == KeyCode.Q)
            {
                currentDirection = RotateCounterClockwise(currentDirection);
                previewObject.transform.rotation = Utilities.DirectionToRotation(currentDirection);
                e.Use();
            }
        }

        if (e.shift)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                if (HasValidPrefab())
                {
                    isDragging = true;
                    currentDragGridPos = gridPos;

                    GameObject prefab = availablePrefabs[selectedPrefabIndex];
                    previewObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    previewObject.transform.position = GridToWorldPosition(gridPos);
                    previewObject.transform.rotation = Utilities.DirectionToRotation(currentDirection);

                    e.Use();
                }
            }
            else if (e.type == EventType.MouseDrag && e.button == 0 && isDragging)
            {
                if (previewObject != null && gridPos != currentDragGridPos)
                {
                    currentDragGridPos = gridPos;
                    previewObject.transform.position = GridToWorldPosition(gridPos);
                }
                e.Use();
            }
            else if (e.type == EventType.MouseUp && e.button == 0 && isDragging)
            {
                if (previewObject != null)
                {
                    if (mapParent != null)
                        previewObject.transform.SetParent(mapParent);

                    Undo.RegisterCreatedObjectUndo(previewObject, "Placed Map Object");

                    // Додаємо об'єкт у список для цієї клітинки
                    AddObjectToGrid(currentDragGridPos, previewObject);

                    previewObject = null;
                }

                isDragging = false;
                e.Use();
            }
            else if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 1)
            {
                RemoveTopObjectAt(gridPos);
                e.Use();
            }
        }
        else
        {
            if (isDragging)
            {
                CancelDragging();
            }
        }
    }

    #region Helper Methods for Direction
    

    private Direction RotationToDirection(Quaternion rotation)
    {
        float yAngle = rotation.eulerAngles.y;
        yAngle = (yAngle + 360f) % 360f;

        if (yAngle >= 45f && yAngle < 135f) return Direction.Right;
        if (yAngle >= 135f && yAngle < 225f) return Direction.Down;
        if (yAngle >= 225f && yAngle < 315f) return Direction.Left;
        return Direction.Up;
    }

    private Direction RotateClockwise(Direction dir)
    {
        return (Direction)(((int)dir + 1) % 4);
    }

    private Direction RotateCounterClockwise(Direction dir)
    {
        return (Direction)(((int)dir + 3) % 4);
    }
    #endregion

    private bool HasValidPrefab()
    {
        return availablePrefabs != null && 
               availablePrefabs.Count > 0 && 
               selectedPrefabIndex < availablePrefabs.Count && 
               availablePrefabs[selectedPrefabIndex] != null;
    }

    private void CancelDragging()
    {
        if (previewObject != null)
        {
            DestroyImmediate(previewObject);
            previewObject = null;
        }
        isDragging = false;
    }

    private Vector3Int WorldToGridPosition(Vector3 worldPos)
    {
        return new Vector3Int(
            Mathf.FloorToInt((worldPos.x + cellSize.x / 2f) / cellSize.x),
            Mathf.FloorToInt((worldPos.y + cellSize.y / 2f) / cellSize.y),
            Mathf.FloorToInt((worldPos.z + cellSize.z / 2f) / cellSize.z)
        );
    }

    private Vector3 GridToWorldPosition(Vector3Int gridPos)
    {
        return new Vector3(
            gridPos.x * cellSize.x,
            gridPos.y * cellSize.y,
            gridPos.z * cellSize.z
        );
    }

    private void AddObjectToGrid(Vector3Int gridPos, GameObject obj)
    {
        if (!spawnedObjects.ContainsKey(gridPos))
        {
            spawnedObjects[gridPos] = new List<GameObject>();
        }
        spawnedObjects[gridPos].Add(obj);
    }

    // Видаляє ОСТАННІЙ доданий об'єкт у цій клітинці
    private void RemoveTopObjectAt(Vector3Int gridPos)
    {
        if (spawnedObjects.TryGetValue(gridPos, out List<GameObject> list) && list.Count > 0)
        {
            int lastIndex = list.Count - 1;
            GameObject objToRemove = list[lastIndex];

            if (objToRemove != null)
            {
                Undo.DestroyObjectImmediate(objToRemove);
            }

            list.RemoveAt(lastIndex);

            if (list.Count == 0)
            {
                spawnedObjects.Remove(gridPos);
            }
        }
    }

    private void ClearSceneMap()
    {
        CancelDragging();

        foreach (var kvp in spawnedObjects)
        {
            foreach (var obj in kvp.Value)
            {
                if (obj != null)
                {
                    DestroyImmediate(obj);
                }
            }
        }
        spawnedObjects.Clear();
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

    private void SaveMapToJson()
    {
        string directory = GetMapDataDirectory();
        string path = EditorUtility.SaveFilePanel("Зберегти мапу", directory, "MapData", "json");
        if (string.IsNullOrEmpty(path)) return;

        MapData mapData = new MapData();
        mapData.cellSize = cellSize;

        foreach (var kvp in spawnedObjects)
        {
            foreach (var obj in kvp.Value)
            {
                if (obj == null) continue;

                GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(obj);
                if (sourcePrefab == null) continue;

                string prefabPath = AssetDatabase.GetAssetPath(sourcePrefab);

                PlacedObjectData data = new PlacedObjectData
                {
                    prefabName = sourcePrefab.name,
                    prefabPath = prefabPath,
                    gridPosition = kvp.Key,
                    direction = RotationToDirection(obj.transform.rotation)
                };

                mapData.placedObjects.Add(data);
            }
        }

        string json = JsonUtility.ToJson(mapData, true);
        File.WriteAllText(path, json);
        AssetDatabase.Refresh();

        Debug.Log($"[Map Editor] Мапу успішно збережено в: {path}");
    }

    private void LoadMapFromJson()
    {
        string directory = GetMapDataDirectory();
        string path = EditorUtility.OpenFilePanel("Завантажити мапу", directory, "json");
        if (string.IsNullOrEmpty(path) || !File.Exists(path)) return;

        ClearSceneMap();

        string json = File.ReadAllText(path);
        MapData mapData = JsonUtility.FromJson<MapData>(json);
        cellSize = mapData.cellSize;

        foreach (var data in mapData.placedObjects)
        {
            GameObject prefab = availablePrefabs.Find(p => p != null && p.name == data.prefabName);

            if (prefab == null && !string.IsNullOrEmpty(data.prefabPath))
            {
                prefab = AssetDatabase.LoadAssetAtPath<GameObject>(data.prefabPath);

                if (prefab != null && !availablePrefabs.Contains(prefab))
                {
                    availablePrefabs.Add(prefab);
                }
            }

            if (prefab != null)
            {
                Vector3 spawnPos = GridToWorldPosition(data.gridPosition);
                GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                newObj.transform.position = spawnPos;
                newObj.transform.rotation = Utilities.DirectionToRotation(data.direction);

                if (mapParent != null)
                    newObj.transform.SetParent(mapParent);

                AddObjectToGrid(data.gridPosition, newObj);
            }
        }

        Repaint();
        Debug.Log($"[Map Editor] Мапу завантажено з: {path}");
    }
}