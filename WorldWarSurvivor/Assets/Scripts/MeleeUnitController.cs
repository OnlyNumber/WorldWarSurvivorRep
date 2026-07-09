using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeUnitController
{
    private Human controllingUnit;

    private BoardGrid boardGrid;

    private ActingObject MyTarget;
    
    //Change this later to getting from controlling unit
    private Weapon weapon;

    private void Initialize(Human human, BoardGrid grid)
    {
        controllingUnit = human;
        boardGrid = grid;
    }

    private void SetTargets(List<ActingObject> actingObjects)
    {
        
    }

    public void CreateQueueOfActions()
    {
        //var path = AStarPathfinding.FindPath(boardGrid, controllingUnit.MyCurrentCell.Coordinate, MyTarget.MyCurrentCell.Coordinate);


    }
}
