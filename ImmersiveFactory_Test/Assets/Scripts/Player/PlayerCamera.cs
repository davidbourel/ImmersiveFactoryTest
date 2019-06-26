using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    [SerializeField] private string mouseXInputName, mouseYInputName;
    [SerializeField] private float mouseSensitivity;

    [SerializeField] private Transform playerBody;

    private float xAxisClamp;

    private bool cameraCanMove = false;

    private float mouseSensibility = 2;

    private void Awake()
    {
        xAxisClamp = 0.0f;
    }

    private void Update()
    {
        if (cameraCanMove)
        {
            CameraRotation();
        }
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity * Time.deltaTime * mouseSensibility;
        float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime * mouseSensibility;

        xAxisClamp += mouseY;

        if (xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }

        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }

    public bool CameraCanMove
    {
        get
        {
            return cameraCanMove;
        }

        set
        {
            cameraCanMove = value;
        }
    }

    public float MouseSensibility
    {
        get
        {
            return mouseSensibility;
        }

        set
        {
            mouseSensibility = value;
        }
    }
}
