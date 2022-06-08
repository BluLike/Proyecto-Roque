using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private Vector3 LastCheckpoint;

    private void Awake()
    {
        LastCheckpoint = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>().LastCheckpointCoord;
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            LastCheckpoint = new Vector3(0,0,0);
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            
        }
    }
    
}
