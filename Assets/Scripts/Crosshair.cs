using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Texture2D crosshairTexture;
    public float crosshairSize = 40f;

    void OnGUI()
    {
        if (crosshairTexture != null)
        {
            float xMin = (Screen.width / 2) - (crosshairSize / 2);
            float yMin = (Screen.height / 2) - (crosshairSize / 2);
            GUI.DrawTexture(new Rect(xMin, yMin, crosshairSize, crosshairSize), crosshairTexture);
        }
    }
}