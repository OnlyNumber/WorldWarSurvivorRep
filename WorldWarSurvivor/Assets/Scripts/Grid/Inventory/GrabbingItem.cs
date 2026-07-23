using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrabbingItem : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler
{
    public bool TargetedObject
    {
        get;
        protected set;
    }

    public Vector3 mouseOffset;

    public RectTransform MyRectTransform;

    public Action OnPickUp;

    public Action OnDrop;

    public Action OnMoving;

    private void Update()
    {
        if (!TargetedObject)
            return;

        MyRectTransform.position = Input.mousePosition + mouseOffset;
        OnMoving?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TargetedObject = !TargetedObject;

        mouseOffset = MyRectTransform.position - Input.mousePosition;

        if (TargetedObject)
        {
            transform.SetAsLastSibling();
            OnPickUp?.Invoke();
        }
        else
        {
            OnDrop?.Invoke();
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (TargetedObject)
        {
            MyRectTransform.position = Input.mousePosition + mouseOffset;
        }
    }

    public void Rotate(Direction direciton)
    {
        if (direciton == Direction.Right)
        {
            MyRectTransform.rotation = Quaternion.Euler(0, 0, 0);
            mouseOffset = new Vector3(-mouseOffset.y, mouseOffset.x);
        }
        else if (direciton == Direction.Up)
        {
            MyRectTransform.rotation = Quaternion.Euler(0, 0, 90);
            mouseOffset = new Vector3(mouseOffset.y, -mouseOffset.x);
        }
    }

    private void OnDestroy()
    {
        OnPickUp = null;
        OnDrop = null;
        OnMoving = null;

    }
}

