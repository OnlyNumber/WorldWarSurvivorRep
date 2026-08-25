using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoveryRoomsMiission : MissionTask
{
    private HashSet<MapCellRoom> mapCellRooms;
    int countOfRooms;

    public override void Initialize(MissionManager missionManager)
    {
        base.Initialize(missionManager);

        missionManager.AddOnMovingToRoom(TryAddToList);
        countOfRooms = missionManager.GetMissionMapController().CountOfCreatedCells;
    }

    public override bool IsMissionCompleted()
    {
        if (mapCellRooms.Count >= countOfRooms * 0.9f)
            return true;

        return false;
    }

    public override void ShowMissionProgress()
    {
        throw new System.NotImplementedException();
    }

    private void TryAddToList(MapCellRoom mapCellRoom)
    {
        mapCellRooms.Add(mapCellRoom);

        if (IsMissionCompleted())
            missionManager.ShowMissionEnd();

    }
}
