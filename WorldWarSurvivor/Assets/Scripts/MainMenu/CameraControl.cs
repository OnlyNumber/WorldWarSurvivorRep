using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraControl
{
    public static Camera MainCamera;

    public static Camera CurrentCamera;




    public static void ChangeToCamera(Camera NextCamera)
    {
        if (NextCamera == null)
            return;

        if (CurrentCamera != null)
            CurrentCamera.gameObject.SetActive(false);

        CurrentCamera = NextCamera;
        CurrentCamera.gameObject.SetActive(true);

    }
}
