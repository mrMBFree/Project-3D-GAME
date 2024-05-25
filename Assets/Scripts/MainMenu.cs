using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenuUI; 

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowOptions()
    {
        optionsMenuUI.SetActive(true);
    }

    public void HideOptions()
    {
        optionsMenuUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Gra zamkniêta!");
    }
}