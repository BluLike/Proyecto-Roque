using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }
    public void StartAgain()
    {
        SceneManager.LoadScene("Nivel Tutorial", LoadSceneMode.Single);
    }
}


