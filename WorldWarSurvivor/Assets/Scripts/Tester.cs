using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public List<ActingObject> AnimatingObjects = new();

    private void Update()
    {
        AnimatingObjects = TurnController.currentMovingObjects.ToList();
    }
}
