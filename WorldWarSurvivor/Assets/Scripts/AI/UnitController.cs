using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController
{
    public virtual void Initialize(Human human, BoardGrid grid)
    {
        
    }

    public virtual List<Action> CreateQueueOfActions()
    {
        return null;
    }
}
