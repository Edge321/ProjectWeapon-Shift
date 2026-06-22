using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    /*private float firstMousePositionX;

    private PlayerMovement playerMovement;
    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        firstMousePositionX = Input.mousePosition.x;
    }
    private void FixedUpdate()
    {
        CameraRotation();
    }*/
    /// <summary>
    /// 
    /// </summary>
    /*private void CameraRotation()
    {
        float secondMousePositionX = Input.mousePosition.x;
        float deltaMouseX = secondMousePositionX - firstMousePositionX;
        float mouseSensitivity = playerMovement.MouseSensitivity;
        if (deltaMouseX > 0)
        {
            transform.rotation *= Quaternion.Euler(0, mouseSensitivity, 0);
            firstMousePositionX = secondMousePositionX;
            playerMovement.Rotating(1);
        }
        else if (deltaMouseX < 0)
        {
            transform.rotation *= Quaternion.Euler(0, -mouseSensitivity, 0);
            firstMousePositionX = secondMousePositionX;
            playerMovement.Rotating(-1);
        }

    }*/
}
