using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MissionTask
{
    protected MissionManager missionManager;
    protected string mapName;

    public abstract bool IsMissionCompleted();

    public virtual void Initialize(MissionManager missionManager, string mapName)
    {
        this.missionManager = missionManager;
        this.mapName = mapName;

    }

    public virtual void ActivateMissionRoom()
    {
        
    }
}
