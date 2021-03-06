using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaRotacion : MonoBehaviour
{


    //Necesitamos el GameObject padre que representa el centro de giro de la torre, el GO del jugador y los GO de las posiciones de salida de la animaci�n (que marcan el lugar 
    //donde el jugador vuelve a tener control.
    public GameObject BaseTorre;
    public GameObject Jugador;
    public GameObject StartRight;
    public GameObject StartLeft;
    public CharacterControllerNonUnity player;


    private bool hasEnter;

    //El Booleano "left" es para marcar si se quiere que se haga un giro a la izquierda, en vez de la derecha, por defecto.
    public bool left;
    private Animator playerAnimaator;


    private void Start()
    {

        playerAnimaator = Jugador.GetComponent<Animator>();

    }

    private void Awake()
    {
        player = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();
    }

    private void Update()
    {
        if (hasEnter == true && left == false)
        {

            StartCoroutine(Rotate(90f));
            hasEnter = false;
            player.currentFace++;
        }

        if (hasEnter == true && left == true)
        {
            StartCoroutine(Rotate(-90f));
            hasEnter = false;
            player.currentFace--;
        }


    }

    IEnumerator Rotate(float rotateAmount)
    {
        //Esta es la rutina que hace que gire. Está dividida en dos partes: La rotacion de la torre y la traslación de la cámara (mediante el jugador oculto).
        player.canMove = false;
        var oldRotation = BaseTorre.transform.rotation;
        BaseTorre.transform.Rotate(0, rotateAmount, 0);
        var newRotation = BaseTorre.transform.rotation;

        var oldTransformPlayer = Jugador.transform.position;
        var newTransformPlayer = new Vector3(StartLeft.transform.position.x, Jugador.transform.position.y + 0.5f,
            StartLeft.transform.position.z);
        if (left == true)
        {
            newTransformPlayer = new Vector3(StartRight.transform.position.x, Jugador.transform.position.y,
                StartRight.transform.position.z);
        }


        for (float t = 0; t <= 1.0; t += Time.deltaTime)
        {
            BaseTorre.transform.rotation = Quaternion.Slerp(oldRotation, newRotation, t);
            Jugador.transform.position = Vector3.Lerp(oldTransformPlayer, newTransformPlayer, t);
            yield return null;
        }

        //characterController.canMove = false;
        BaseTorre.transform.rotation = newRotation;
        Jugador.transform.position = newTransformPlayer;
        player.canMove = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            hasEnter = true;


        }
    }
}