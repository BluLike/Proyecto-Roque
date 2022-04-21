using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
           
            
                SceneManager.LoadScene("End", LoadSceneMode.Single);
            
        }
    }
}
