using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] protected GameObject window;


    public virtual void Show()
    {
        window.SetActive(true);
    }
    
    public virtual void Hide()
    {
        window.SetActive(false);
        
    }
}
