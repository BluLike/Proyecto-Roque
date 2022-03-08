using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaRotacion : MonoBehaviour
{
    public Quaternion rotation=Quaternion.Euler(90f,0f,0f);
    public float rotationSpeed = 1;


    // Update is called once per frame

    private void Update()
    {
        if (Input.GetButton("Jump"))
        {
            PruebaRotation();
        }
    }
    void PruebaRotation()
    {
       transform.rotation=Quaternion.Lerp(transform.rotation,rotation , Time.deltaTime* rotationSpeed); 
    }
}
