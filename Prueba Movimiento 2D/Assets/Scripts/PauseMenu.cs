using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject PauseMenuUI;

    CharacterControllerNonUnity characterControllerNonUnity;

    //string Life = "Life";

    //int SaveLife;

    // Update is called once per frame

    private void Awake()
    {
        characterControllerNonUnity = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();
    }
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
        Cursor.visible = false;
        Time.timeScale = 1;
        GameIsPaused = false;

    }
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0;
        GameIsPaused = true;
    }
    public void Exit()
    {
        Time.timeScale = 1;
        GameIsPaused = false;
        SceneManager.LoadScene("Menu");
    }
}
