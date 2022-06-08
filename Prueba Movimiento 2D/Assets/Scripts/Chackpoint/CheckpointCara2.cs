using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointCara2 : MonoBehaviour
{

    //Animation states
    private const string OFF = "Apagado";
    private const string ON = "Encendido";

    private bool apagado = true;
    private DataPersistenceManager SaveManager;


    //Animator parameters
	
    [SerializeField] private Animator animator1;
    [SerializeField] private Animator animator2;
    private string currentState;

    public void Awake()
    {
        SaveManager = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();
    }

    void ChangeAnimationState(string newState)
    {
        //evita que se quede en bucle la animación
        if (currentState == newState) return;
        //hace la animación 
        animator1.Play(newState);
        animator2.Play(newState);
        //asignar nuevo valor de la animación
        currentState = newState;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (apagado)
            {
                ChangeAnimationState(ON); 
            }
            SaveManager.SaveGame();
            other.gameObject.GetComponent<CharacterControllerNonUnity>().LastCheckpointCoord = gameObject.transform.position;
            Debug.Log(other.gameObject.GetComponent<CharacterControllerNonUnity>().LastCheckpointCoord);
        }
    }
}
