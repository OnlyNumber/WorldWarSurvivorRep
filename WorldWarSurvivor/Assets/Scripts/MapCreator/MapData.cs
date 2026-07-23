using System;
using System.Collections.Generic;
using UnityEngine;

// Дані про один спавнений об'єкт
[Serializable]
public class PlacedObjectData
{
    public string prefabName; // Назва префабу
    public string prefabPath;
    public Vector3Int gridPosition; // Позиція на сітці (X, Y, Z)
    public Direction direction; // Поворот об'єкта
}

// Загальна структура мапи
[Serializable]
public class MapData
{
    public Vector3 cellSize = new Vector3(1f, 1f, 1f); // Розмір однієї клітинки
    public List<PlacedObjectData> placedObjects = new List<PlacedObjectData>();
}