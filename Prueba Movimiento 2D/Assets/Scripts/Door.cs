using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
<<<<<<< Updated upstream
    void OnTriggerStay(Collider collider)
=======
    void OnColliderStay(Collision collision)
>>>>>>> Stashed changes
    {
        if (collider.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            
                SceneManager.LoadScene("End", LoadSceneMode.Single);
            
        }
    }
    
}
