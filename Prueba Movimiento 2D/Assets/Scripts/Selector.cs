using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Selector : MonoBehaviour
{
    public void Tutorial()
    {
        SceneManager.LoadScene("Nivel Tutorial");
    }
    public void PrimerNivel()
    {
        SceneManager.LoadScene("Nivel_P_1");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}