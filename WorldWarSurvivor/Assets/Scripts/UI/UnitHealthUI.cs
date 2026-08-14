using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthUI : MonoBehaviour
{
    public TMP_Text HealthText;

    public Slider HealthBar;

    public void SetBar(string text, float fillOfBar)
    {
        HealthText.text = text;
        HealthBar.value = fillOfBar;
    }

    private void Update() 
    {
        transform.rotation = Quaternion.Euler(new Vector3(30,0,0));
    }
}
