using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{

    public static bool victoryPause = false;
    public GameObject victoryUI;

    // Update is called once per frame
    void Update()
    {

    }

    public void victoryScreen()
    {
        FindObjectOfType<AudioManager>().Stop("SceneMusic");
        FindObjectOfType<AudioManager>().Stop("MainTheme");
        if (victoryPause)
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
        victoryUI.SetActive(false);
        Time.timeScale = 1f;
        victoryPause = false;
    }

    void PauseOver()
    {
        victoryUI.SetActive(true);
        Time.timeScale = 0f;
        victoryPause = true;
    }

    public void ResumeVictory()
    {
        victoryUI.SetActive(false);
        Time.timeScale = 1f;
        victoryPause = false;
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
