using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public GameObject doorClosed;
    public GameObject doorOpen;
    public float interactionDistance = 3.0f;
    private bool isDoorOpen = false;
    public LayerMask playerLayer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckForPlayerAndToggleDoor();
        }
    }

    private void CheckForPlayerAndToggleDoor()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionDistance, playerLayer);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                ToggleDoor();
                break;
            }
        }
    }

    private void ToggleDoor()
    {
        if (isDoorOpen)
        {
            Debug.Log("Closing door");
            doorClosed.SetActive(true);
            doorOpen.SetActive(false);
            isDoorOpen = false;
        }
        else
        {
            Debug.Log("Opening door");
            doorClosed.SetActive(false);
            doorOpen.SetActive(true);
            isDoorOpen = true;
        }
    }
}