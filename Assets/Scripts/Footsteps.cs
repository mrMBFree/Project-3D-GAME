using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip[] footstepSounds; 
    public float stepInterval = 0.5f; 
    private float nextStepTime = 0f;

    void Update()
    {
        
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            
            if (Time.time >= nextStepTime)
            {
                PlayFootstepSound();
                nextStepTime = Time.time + stepInterval; 
            }
        }
    }

    void PlayFootstepSound()
    {
        if (footstepSounds.Length > 0)
        {
            
            int index = Random.Range(0, footstepSounds.Length);
            audioSource.clip = footstepSounds[index];
            audioSource.Play();
        }
    }
}