using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitDescription : MonoBehaviour
{
    public static HitDescription Instance;

    public TMP_Text HealthText;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        gameObject.SetActive(false);
    }

    public void Show(string text)
    {
        gameObject.SetActive(true);
        transform.rotation = Quaternion.Euler(new Vector3(30, 0, 0));
        HealthText.text = text;
    }

    public void Hide()
    {
        gameObject.SetActive(false);

    }

    public void HideAfterDelay(float delay = 3)
    {
        StartCoroutine(Utilities.WaitAndRun(Hide, delay));
    }


}
