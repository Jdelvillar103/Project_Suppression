using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    //When Play button is pressed, it loads next scene (tutorial level)
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    //When Quit button is pressed, application is exited out
    public void QuitGame()
    {
        //Debug "Quiting Game!" in log because we cannot check application closing in Unity
        Debug.Log("Quiting Game!");
        Application.Quit();
    }
}
