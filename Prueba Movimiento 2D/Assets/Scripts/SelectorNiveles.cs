using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SelectorNiveles : MonoBehaviour, IDataPersistence
{
    public DataPersistenceManager dataPersistenceManager;
    GameData gameData;
    [SerializeField] string currentScene;
    private void Start()
    {
        HALO();
    }
    public void LoadData(GameData data)
    {
        this.currentScene = data.currentScene;
    }
    public void SaveData(GameData data)
    {

    }
    void HALO()
    {
        Debug.Log("Cargando Escena Guardada");
        dataPersistenceManager = GetComponent<DataPersistenceManager>();
        dataPersistenceManager = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        dataPersistenceManager.LoadGame();
        StartCoroutine(CargarEscenaCorrespondiente());
        }
    IEnumerator CargarEscenaCorrespondiente()
    {
        yield return new WaitForSeconds(2f);
        SCENA();
    }
    void SCENA()
    {
        Debug.Log("Escena cargada");
        SceneManager.LoadSceneAsync(this.currentScene);
    }
}

