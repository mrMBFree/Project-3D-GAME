using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; 
    public MonoBehaviour FirstPersonController;
    private bool isPaused = false;
    public GameObject optionsMenuUI;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
        FirstPersonController.enabled = true;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

   
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; 
        isPaused = true;
        FirstPersonController.enabled = false;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    }
   /* public void ShowOptions()
    {
        optionsMenuUI.SetActive(true);
    }

    public void HideOptions()
    {
        optionsMenuUI.SetActive(false);
    }
   */
 
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }

  
    public void QuitGame()
    {
        Time.timeScale = 1f; 
        Application.Quit();
        Debug.Log("Gra zamkniêta!");
    }
}