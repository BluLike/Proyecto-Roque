using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IDataPersistence
{
    private Vector3 LastCheckpoint;
    bool newlvl = false;

    public void LoadData(GameData data)
    {

    }

    public void SaveData(GameData data)
    {
        if (newlvl == true)
        {
            data.currentFace = 1;
            data.LastCheckpointCoord = new Vector3(0,0,0);
        }
    }
    private void Awake()
    {
        LastCheckpoint = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>().LastCheckpointCoord;
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            newlvl = true;
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    
}
