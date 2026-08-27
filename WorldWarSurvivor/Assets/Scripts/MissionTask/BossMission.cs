using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMission : MissionTask
{
    private EnemyBand[] _bossFightBands;
    private MapCellRoom _fightRoom;
    private bool _isMissionCompleted;


    public override void Initialize(MissionManager missionManager, string mapName)
    {
        base.Initialize(missionManager, mapName);

        AddBossMission();

        Resources.Load<EnemyBand>(Utilities.Resources_Path_To_Data + "/" + mapName + "/Bands/BossBands");
        missionManager.OnShowRewardFromFight += CheckReward;

    }

    public void AddBossMission()
    {
        List<MapCellRoom> fightRooms = new();

        foreach (var room in missionManager.GetMissionMapController().GetAllCreatedRooms())
        {
            if (room.activity == Activities.Battle)
                fightRooms.Add(room);
        }

        _fightRoom = fightRooms[Random.Range(0, fightRooms.Count)];
        _fightRoom.activity = Activities.MissionRoom;
    }

    public override bool IsMissionCompleted()
    {
        return _isMissionCompleted;
    }


    public void CheckReward(MapCellRoom room)
    {
        _isMissionCompleted = room == _fightRoom;
        if (IsMissionCompleted())
            missionManager.ShowMissionEnd();

    }

    public override void ActivateMissionRoom()
    {
        base.ActivateMissionRoom();

        missionManager.CreateMissionBattle(_bossFightBands[0]);

    }
}
