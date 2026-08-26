using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CommandMover : MonoBehaviour
{
    [SerializeField] private MapGrid mapGrid;
    private float speed = 0.5f;
    private List<MapCellRoom> _neighbourCells = new();

    public RectTransform CommandIcon;
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
                cell.SetForOgWar(false);
                cell.MyButton.onClick.AddListener(() => MoveToThisRoom(cell));

                _neighbourCells.Add(cell);
            }
        }

        MoveIcon(nextRoom);
    }

    private void MoveIcon(MapCellRoom room)
    {
        var sequence = DOTween.Sequence()
        .Append(CommandIcon.transform.DOMove(room.transform.position, speed));

        sequence.onComplete = () => OnMovingToRoom?.Invoke(room);
    }

}
