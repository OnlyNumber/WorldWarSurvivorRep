using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMission : MissionTask
{
    private EnemyBand bossFightBand;

    public override void Initialize(MissionManager missionManager)
    {
        base.Initialize(missionManager);

        AddBossMission();
        //missionManager.AddOnMovingToRoom(TryAddToList);
    }

    public void AddBossMission()
    {
        List<MapCellRoom> fightRooms = new();

        foreach (var room in missionManager.GetMissionMapController().GetAllCreatedRooms())
        {
            if (room.activity == Activities.Battle)
                fightRooms.Add(room);
        }

        fightRooms[Random.Range(0, fightRooms.Count)].activity = Activities.MissionRoom;
    }

    public override bool IsMissionCompleted()
    {
        throw new System.NotImplementedException();
    }

    public override void ShowMissionProgress()
    {
        throw new System.NotImplementedException();
    }

    public override void ActivateMissionRoom()
    {
        base.ActivateMissionRoom();

        missionManager.CreateMissionBattle(bossFightBand);

    }
}
