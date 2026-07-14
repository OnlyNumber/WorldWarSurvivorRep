using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseProgression : MonoBehaviour
{
    public static BaseProgression Instance;

    public List<HumanStats> MyHumans = new();

    private void Start()
    {
        if(Instance == null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        LoadInfo();
    }



    private void LoadInfo()
    {

    }
}
