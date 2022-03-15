using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaRotacion : MonoBehaviour
{
   

    public GameObject BaseTorre;
    public GameObject Jugador;
    public GameObject TransiAnim;
    public bool hasEnter;
    public bool left;
   
    
    private void Update()
    {
        if (hasEnter==true&&left==false)
        {
            StartCoroutine(Rotate(90f));
            hasEnter = false;
        }
        if (hasEnter == true && left == true)
        {
            StartCoroutine(Rotate(-90f));
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
