using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource audioSource; // AudioSource dodany do postaci
    public AudioClip[] footstepSounds; // Tablica z d�wi�kami krok�w
    public float stepInterval = 0.5f; // Czas mi�dzy krokami

    private float nextStepTime = 0f;

    void Update()
    {
        // Sprawdzanie czy posta� si� porusza (przyklad dla postaci sterowanej klawiatur�)
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            // Je�li nadszed� czas na kolejny krok
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
            // Wybierz losowy d�wi�k z tablicy
            int index = Random.Range(0, footstepSounds.Length);
            audioSource.clip = footstepSounds[index];
            audioSource.Play();
        }
    }
}