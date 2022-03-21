using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaRotacion : MonoBehaviour
{
   

    public GameObject BaseTorre;
    public GameObject Jugador;
    public GameObject StartRight;
    public GameObject StartLeft;
    public bool hasEnter;
    public bool left;
    private Animator playerAnimaator;


    private void Start()
    {
           playerAnimaator = Jugador.GetComponent<Animator>();
    }
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
        Jugador.SetActive(false);
        var oldRotation = BaseTorre.transform.rotation;
        BaseTorre.transform.Rotate(0, rotateAmount, 0);
        var newRotation = BaseTorre.transform.rotation;

        var oldTransformPlayer=Jugador.transform.position;
        
        var newTransformPlayer = StartLeft.transform.position;
        

        for (float t = 0; t <= 1.0; t += Time.deltaTime)
        {
            BaseTorre.transform.rotation = Quaternion.Slerp(oldRotation, newRotation, t);
            Jugador.transform.position = Vector3.Lerp(oldTransformPlayer, newTransformPlayer, t);
            yield return null;
        }
        BaseTorre.transform.rotation = newRotation;
        Jugador.transform.position = newTransformPlayer;
        Jugador.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
            hasEnter = true;
            
            
        }
    }
}
