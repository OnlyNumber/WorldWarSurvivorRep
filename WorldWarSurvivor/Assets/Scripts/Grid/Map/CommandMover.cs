using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMover : MonoBehaviour
{
    public RectTransform CommandIcon;
    [SerializeField] private MapGrid mapGrid;
    private List<MapCellRoom> _neighbourCells = new();
    public Action<MapCellRoom> OnMovingToRoom;

    public void MoveToThisRoom(MapCellRoom nextRoom)
    {
        //_currentRoom = nextRoom;
        foreach (var cell in _neighbourCells)
            cell.MyButton.onClick.RemoveAllListeners();

        foreach (var direction in mapGrid.Directions)
        {
            var cell = mapGrid.GetCell(nextRoom.Coordinate + direction);
            if (cell != null && cell.IsCreated)
            {
                _neighbourCells.Add(cell);
                cell.MyButton.onClick.AddListener(() => MoveToThisRoom(cell));
            }
        }

        OnMovingToRoom?.Invoke(nextRoom);
        MoveIcon(nextRoom.MyRectTransform.position);
    }

    private void MoveIcon(Vector2 position)
    {
        CommandIcon.position = position;
    }

}
