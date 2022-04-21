using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject PauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
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
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
    }
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
    }
    public void Exit()
    {
        Application.Quit();
    }
}
