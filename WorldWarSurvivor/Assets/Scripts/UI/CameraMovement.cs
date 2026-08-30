using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed;
    public float speedMultiplyer;
    public Transform CameraTransform;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        float currentSpeed = speed;

        if (Input.GetKey(KeyCode.LeftShift))
            currentSpeed *= speedMultiplyer;

        CameraTransform.position += new Vector3(x, 0, y) * currentSpeed * Time.deltaTime;
    }
}
