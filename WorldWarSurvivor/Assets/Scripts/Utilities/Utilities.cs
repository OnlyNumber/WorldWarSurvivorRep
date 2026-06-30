using System;
using System.Collections;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public static IEnumerator WaitAndRun(Action action, float time = 0.1f)
    {
        yield return new WaitForSeconds(time);

        action.Invoke();
    }
}
