using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrabbingItem : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler
{
    public bool targetedObject;

    public Vector3 mouseOffset;

    public RectTransform MyRectTransform;

    public Action<Item> OnPickUp;

    public Action<Item> OnDrop;


    private void Start()
    {
        MyRectTransform = GetComponent<RectTransform>();
        StartCoroutine(Utilities.WaitAndRun(Subscribe, 0.2f));
    }

    private void Subscribe()
    {
        OnPickUp += InventorySystem.Instance.PickUpItem;
        OnDrop += InventorySystem.Instance.DropItem;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        targetedObject = !targetedObject;

        mouseOffset = MyRectTransform.position - Input.mousePosition;
        
        if (targetedObject)
        {
            OnPickUp?.Invoke(GetComponent<Item>());
        }
        else
        {
            OnDrop?.Invoke(GetComponent<Item>());
        }


    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (targetedObject)
        {
            MyRectTransform.position = Input.mousePosition + mouseOffset;
        }
    }
}
