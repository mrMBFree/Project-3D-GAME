using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource audioSource; // AudioSource dodany do postaci
    public AudioClip[] footstepSounds; // Tablica z dŸwiêkami kroków
    public float stepInterval = 0.5f; // Czas miêdzy krokami

    private float nextStepTime = 0f;

    void Update()
    {
        // Sprawdzanie czy postaæ siê porusza (przyklad dla postaci sterowanej klawiatur¹)
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            // Jeœli nadszed³ czas na kolejny krok
            if (Time.time >= nextStepTime)
            {
                PlayFootstepSound();
                nextStepTime = Time.time + stepInterval; // Ustaw czas na kolejny krok
            }
        }
    }

    void PlayFootstepSound()
    {
        if (footstepSounds.Length > 0)
        {
            // Wybierz losowy dŸwiêk z tablicy
            int index = Random.Range(0, footstepSounds.Length);
            audioSource.clip = footstepSounds[index];
            audioSource.Play();
        }
    }
}