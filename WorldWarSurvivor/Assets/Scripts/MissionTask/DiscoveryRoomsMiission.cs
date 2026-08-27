using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoveryRoomsMiission : MissionTask
{
    private HashSet<MapCellRoom> mapCellRooms = new();
    int countOfRooms;

    public override void Initialize(MissionManager missionManager, string mapName)
    {
        base.Initialize(missionManager, mapName);

        missionManager.AddOnMovingToRoom(TryAddToList);
        countOfRooms = missionManager.GetMissionMapController().CountOfCreatedCells;
    }

    public override bool IsMissionCompleted()
    {
        if (mapCellRooms.Count >= countOfRooms * 0.9f)
            return true;

        return false;
    }

    private void TryAddToList(MapCellRoom mapCellRoom)
    {
        mapCellRooms.Add(mapCellRoom);

        if (IsMissionCompleted())
            missionManager.ShowMissionEnd();

    }
}
