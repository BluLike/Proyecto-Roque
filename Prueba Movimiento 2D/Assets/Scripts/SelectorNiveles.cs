using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorNiveles : MonoBehaviour
{
    public void Tutorial()
    {
        SceneManager.LoadScene("Nivel Tutorial");
    }
    public void PrimerNivel()
    {
        SceneManager.LoadScene("Nivel_P_1");
    }
    public void SegundoNivel()
    {
        SceneManager.LoadScene("Nivel_C_1");
    } 
    public void TercerNivel()
    {
        SceneManager.LoadScene("Nivel_P_2");
    }
    public void CuartoNivel()
    {
        SceneManager.LoadScene("Nivel_C_2");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}