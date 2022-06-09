using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    public GameObject warningText;

    private void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            warningText.SetActive(false);
            continueGameButton.interactable = false;
        }
        if (DataPersistenceManager.instance.HasGameData())
        {
            warningText.SetActive(true);
        }

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene current)
    {
        DataPersistenceManager.instance.SaveGame();
        Debug.Log("funciono puta");
    }

    public void OnNewGameClicked()
    {
        
        DisableMenuButtons();
        // create a new game - which will initialize our game data
        DataPersistenceManager.instance.NewGame();
        // load the gameplay scene - which will in turn save the game because of
        // OnSceneUnloaded() in the DataPersistenceManager
        SceneManager.LoadScene("Selector");
    }

    public void OnContinueGameClicked()
    {
        DisableMenuButtons();
        // load the next scene - which will in turn load the game because of 
        // OnSceneLoaded() in the DataPersistenceManager
        SceneManager.LoadScene("Loading");
    }

    public void ExitGame()
    {
        DisableMenuButtons();
        Application.Quit();
    }
    
    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }
}