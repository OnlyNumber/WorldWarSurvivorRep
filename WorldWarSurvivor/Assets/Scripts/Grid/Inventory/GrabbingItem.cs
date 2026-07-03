using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrabbingItem : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler
{
    public bool targetedObject;

    public Vector3 mouseOffset;

    public RectTransform MyRectTransform;

    public Action<InventoryItem> OnPickUp;

    public Action<InventoryItem> OnDrop;

    public Action<InventoryItem> OnMoving;

    private InventoryItem inventoryItem;

    private void Start()
    {
        MyRectTransform = GetComponent<RectTransform>();
        inventoryItem = GetComponent<InventoryItem>();
        StartCoroutine(Utilities.WaitAndRun(Subscribe, 0.2f));
    }

    private void Update()
    {
        if (targetedObject)
        {
            MyRectTransform.position = Input.mousePosition + mouseOffset;
            OnMoving?.Invoke(inventoryItem);
        }

        if (Input.GetKeyDown(KeyCode.E) && targetedObject)
        {
            if (inventoryItem.direciton == Direciton.Right)
            {
                MyRectTransform.rotation = Quaternion.Euler(0, 0, 90);
                inventoryItem.direciton = Direciton.Up;
                mouseOffset = new Vector3(-mouseOffset.y, mouseOffset.x);
            }
            else
            {
                MyRectTransform.rotation = Quaternion.Euler(0, 0, 0);
                inventoryItem.direciton = Direciton.Right;
                mouseOffset = new Vector3(mouseOffset.y, -mouseOffset.x);
            }

        }
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
            transform.SetAsLastSibling();
            OnPickUp?.Invoke(inventoryItem);
        }
        else
        {
            OnDrop?.Invoke(inventoryItem);
        }
    }



    public void OnPointerMove(PointerEventData eventData)
    {
        if (targetedObject)
        {
            MyRectTransform.position = Input.mousePosition + mouseOffset;
        }
    }

    private void OnDestroy() 
    {
        OnPickUp += InventorySystem.Instance.PickUpItem;
        OnDrop += InventorySystem.Instance.DropItem;
    }
}
