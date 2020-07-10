using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    public static bool gameOverPause = false;
    public GameObject gameOverUI;

	// Update is called once per frame
	void Update () {
		
	}

     public void GameOverScreen()
    {
        FindObjectOfType<AudioManager>().Stop("SceneMusic");
        FindObjectOfType<AudioManager>().Stop("MainTheme");
        if (gameOverPause)
        {
            ResumeOver();
        }
        else
        {
            Cursor.visible = true;
            PauseOver();
        }
    }

    void ResumeOver()
    {
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;
        gameOverPause = false;
    }

    void PauseOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        gameOverPause = true;
    }

    public void ResumeGame()
    {
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;
        gameOverPause = false;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        Debug.Log("Loading menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
