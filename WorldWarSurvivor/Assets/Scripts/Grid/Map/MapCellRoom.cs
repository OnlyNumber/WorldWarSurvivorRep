using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCellRoom : Cell
{
     public Image RoomBackground;
     public Image roomIcon;
     public Activities activity = Activities.Nothing;
     public bool IsCreated = false;
     public RectTransform MyRectTransform;
     public Button MyButton;

     public void SetupRoom(Activities activity, Sprite roomIcon)
     {
          this.activity = activity;
          this.roomIcon.sprite = roomIcon;

     }
     
}

public enum Activities
{
     Nothing,
     Battle,
     Shop,
     Event
}
