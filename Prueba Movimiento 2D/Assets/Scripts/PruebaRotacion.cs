using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaRotacion : MonoBehaviour
{
    /* public GameObject BaseTorre;

     public Quaternion newAngle;



     private void Update()
     {
         //targetAngle = Quaternion.Euler(0,currentAngle.eulerAngles.y+90,0);


         //yield return null;
     }

     private void OnTriggerEnter(Collider other)
     {
         if (other.tag == "Player")
         {
             newAngle = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 90f, 0);
             Debug.Log("Colision");
             //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, transform.rotation.y + 90f, 0f), Time.deltaTime*0.01f);

             //BaseTorre.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.y + 90, 0),0.2f+Time.deltaTime);
             //yield return null;
             for (float t = 0; t <= 1.0; t += Time.deltaTime)
             {
                 BaseTorre.transform.rotation = Quaternion.Slerp(transform.rotation,newAngle , t);
             }

         }
     } */

    public GameObject BaseTorre;
    public bool hasEnter;
   
    
    private void Update()
    {
        if (hasEnter==true)
        {
            StartCoroutine(Rotate(90f));
            hasEnter = false;
        }
    }

    private IEnumerator Rotate(float rotateAmount)
    {
        var oldRotation = BaseTorre.transform.rotation;
        BaseTorre.transform.Rotate(0, rotateAmount, 0);
        var newRotation = BaseTorre.transform.rotation;

        for (float t = 0; t <= 1.0; t += Time.deltaTime)
        {
            BaseTorre.transform.rotation = Quaternion.Slerp(oldRotation, newRotation, t);
            yield return null;
        }
        BaseTorre.transform.rotation = newRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            hasEnter = true;
        }
    }
}
