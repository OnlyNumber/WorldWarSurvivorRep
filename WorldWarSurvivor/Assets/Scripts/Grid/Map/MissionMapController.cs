using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class MissionMapController
{
    private const int CountRoomsPerLong = 5;
    private const int Nothing_Chance = 20;
    private const int Battle_Chance = 50;
    private const int Shop_Chance = 10;
    private const int Event_Chance = 20;


    public MapGrid CurrentMap;
    public Vector2Int SizeOfGrid;
    [Tooltip("1 Nothing, 2 Battle, 3 Shop, 4 Event ")]
    public List<Sprite> RoomIcons = new();


    private HashSet<MapCellRoom> _visitedCells = new();
    private Queue<MapCellRoom> _cellsForVisit = new();
    private int _needCells;
    private int _currentCountOfCells;

    List<Vector2Int> Directions = new(){
            new Vector2Int(0,1),
            new Vector2Int(1,0),
            new Vector2Int(0,-1),
            new Vector2Int(-1,0)
        };

    public MapCellRoom GetStartCell() => CurrentMap.GetCell(SizeOfGrid.x / 2, SizeOfGrid.y / 2);

    public void SetLongOfMap(MissionLong missionLong)
    {
        _needCells = (int)missionLong * CountRoomsPerLong;
        _needCells += Random.Range(1, 4);
    }

    public void CreateMap()
    {
        SetupMap();

        for (int x = 0; x < CurrentMap.GridSize.x; x++)
        {
            for (int y = 0; y < CurrentMap.GridSize.y; y++)
            {
                var cell = CurrentMap.GetCell(x, y);
                if (_visitedCells.Contains(cell))
                    continue;

                cell.gameObject.SetActive(false);
            }
        }
    }

    private void SetupMap()
    {
        CurrentMap.ClearCells();
        _cellsForVisit.Clear();
        _visitedCells.Clear();
        _currentCountOfCells = 0;

        CurrentMap.CreateGrid(SizeOfGrid.x, SizeOfGrid.y);

        GenerateMap();
    }

    public void GenerateMap()
    {
        CurrentMap.GetCell(SizeOfGrid.x / 2, SizeOfGrid.y / 2);

        VisitCell(CurrentMap.GetCell(SizeOfGrid.x / 2, SizeOfGrid.y / 2), 1);
        MapCellRoom currentCell;

        CurrentMap.GetCell(SizeOfGrid.x / 2, SizeOfGrid.y / 2).SetupRoom(Activities.Nothing, RoomIcons[(int)Activities.Nothing]);

        do
        {
            if (_cellsForVisit.Count == 0)
                break;
            currentCell = _cellsForVisit.Dequeue();

            foreach (var dir in Directions)
            {
                var dirCell = CurrentMap.GetCell(currentCell.Coordinate + dir);
                if (dirCell != null)
                    VisitCell(dirCell, Random.value);
            }

        } while (_currentCountOfCells <= _needCells);

        if (_currentCountOfCells < _needCells)
        {
            SetupMap();
            return;
        }
    }
    
    private bool VisitCell(MapCellRoom room, float value)
    {

        if (room.IsCreated || _currentCountOfCells >= _needCells || value < 0.3f || !IsAccessibleNeighbours(room.Coordinate))
            return false;

        _cellsForVisit.Enqueue(room);
        _currentCountOfCells++;
        room.IsCreated = true;

        int roomChance = Random.Range(0, 100);

        if (roomChance <= Nothing_Chance)
            room.SetupRoom(Activities.Nothing, RoomIcons[(int)Activities.Nothing]);

        else if (roomChance <= Nothing_Chance + Battle_Chance)
            room.SetupRoom(Activities.Battle, RoomIcons[(int)Activities.Battle]);

        else if (roomChance <= Nothing_Chance + Battle_Chance + Shop_Chance)
            room.SetupRoom(Activities.Shop, RoomIcons[(int)Activities.Shop]);

        else if (roomChance <= Nothing_Chance + Battle_Chance + Shop_Chance + Event_Chance)
            room.SetupRoom(Activities.Event, RoomIcons[(int)Activities.Event]);

        _visitedCells.Add(room);

        return true;

    }

    private bool IsAccessibleNeighbours(Vector2Int coordinate)
    {
        int count = 0;

        foreach (var dir in Directions)
        {
            var cell = CurrentMap.GetCell(coordinate + dir);

            if (cell == null || cell.activity == Activities.Nothing)
                continue;

            count++;
        }


        if (count > 1)
        {
            if (count == 2)
            {
                if (CheckSides(coordinate, Directions[0], Directions[2]) || CheckSides(coordinate, Directions[1], Directions[3]))
                    return true;
            }

            return false;
        }


        return true;

        bool CheckSides(Vector2Int coordinate, Vector2Int direction1, Vector2Int direction2)
        {
            var cell1 = CurrentMap.GetCell(coordinate + direction1);
            var cell2 = CurrentMap.GetCell(coordinate + direction2);

            if (cell1 != null && cell2 != null && cell1.activity != Activities.Nothing && cell2.activity != Activities.Nothing)
                return true;

            return false;

        }
    }
    
}
