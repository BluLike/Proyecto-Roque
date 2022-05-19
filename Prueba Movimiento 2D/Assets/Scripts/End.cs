using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public void Exit()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    public void StartAgain()
    {
        SceneManager.LoadScene("Nivel_P_1", LoadSceneMode.Single);
    }
}


