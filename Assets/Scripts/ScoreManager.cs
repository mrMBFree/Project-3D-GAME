using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TMP_Text scoreText; // U�ywanie TMP_Text zamiast Text
    public int scoreLength = 7; // D�ugo�� wy�wietlanego wyniku

    // Metoda do dodawania punkt�w
    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreText();
    }

    // Aktualizacja tekstu punktacji na UI
    void UpdateScoreText()
    {
        scoreText.text = score.ToString().PadLeft(scoreLength, '0');
    }
}