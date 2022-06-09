using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointFinal : MonoBehaviour
{

    
  
    private DataPersistenceManager SaveManager;


    //Animator parameters

    

    public void Awake()
    {
        SaveManager = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SaveManager.SaveGame();
            other.gameObject.GetComponent<CharacterControllerNonUnity>().currentFace = 1;
            other.gameObject.GetComponent<CharacterControllerNonUnity>().LastCheckpointCoord = new Vector3(0,0,0);
            Debug.Log(other.gameObject.GetComponent<CharacterControllerNonUnity>().LastCheckpointCoord);
        }
    }
}
