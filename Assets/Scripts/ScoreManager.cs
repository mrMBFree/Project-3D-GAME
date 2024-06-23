using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TMP_Text scoreText; 
    public int scoreLength = 7; 

    
    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreText();
    }

    
    void UpdateScoreText()
    {
        scoreText.text = score.ToString().PadLeft(scoreLength, '0');
    }
}