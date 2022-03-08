using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaRotacion_A : MonoBehaviour
{
    public GameObject BaseTorre;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Colision");
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, transform.rotation.y + 90f, 0f), Time.deltaTime*0.01f);

            BaseTorre.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.rotation.y + 90, 0),Time.time*0.1f);
        }
    }
}
