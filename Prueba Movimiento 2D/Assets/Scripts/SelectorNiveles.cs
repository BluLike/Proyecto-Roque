using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorNiveles : MonoBehaviour
{
    public void Tutorial()
    {
        SceneManager.LoadScene("Nivel Tutorial", LoadSceneMode.Single);
    }
    public void PrimerNivel()
    {
        SceneManager.LoadScene("Nivel_P_1", LoadSceneMode.Single);
    }
    public void SegundoNivel()
    {
        SceneManager.LoadScene("Nivel_C_1", LoadSceneMode.Single);
    } 
    public void TercerNivel()
    {
        SceneManager.LoadScene("Nivel_P_2", LoadSceneMode.Single);
    }
    public void CuartoNivel()
    {
        SceneManager.LoadScene("Nivel_C_2", LoadSceneMode.Single);
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}