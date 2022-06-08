using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private Transform LastCheckpoint;

    private void Awake()
    {
        LastCheckpoint = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>().LastCheckpointTransform;
    }

    void OnTriggerStay(Collider collider)

    {
        if (collider.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            LastCheckpoint = null;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            
        }
    }
    
}
