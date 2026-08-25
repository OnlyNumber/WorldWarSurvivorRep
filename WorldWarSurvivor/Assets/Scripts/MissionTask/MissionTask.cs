using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MissionTask
{
    protected MissionManager missionManager;

    public abstract bool IsMissionCompleted();

    public virtual void Initialize(MissionManager missionManager)
    {
        this.missionManager = missionManager;
    }

    public abstract void ShowMissionProgress();

    public virtual void ActivateMissionRoom()
    {
        
    }
}
