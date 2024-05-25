using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera firstPersonCamera; 
    public Camera thirdPersonCamera; 

    public Transform thirdPersonCameraTarget; 
    public float lookSpeed = 2f; 
    public float lookXLimit = 45f; 

    private bool isThirdPersonView = false; 
    private float rotationX = 0;

    void Start()
    {
        firstPersonCamera.gameObject.SetActive(true);
        thirdPersonCamera.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchView();
        }


        if (isThirdPersonView)
        {
            HandleThirdPersonCameraRotation();
        }
    }


    void SwitchView()
    {
        isThirdPersonView = !isThirdPersonView;

        if (isThirdPersonView)
        {
            firstPersonCamera.gameObject.SetActive(false);
            thirdPersonCamera.gameObject.SetActive(true);
        }
        else
        {
            firstPersonCamera.gameObject.SetActive(true);
            thirdPersonCamera.gameObject.SetActive(false);
        }
    }


    void HandleThirdPersonCameraRotation()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        thirdPersonCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);


        thirdPersonCamera.transform.position = thirdPersonCameraTarget.position;
    }
}