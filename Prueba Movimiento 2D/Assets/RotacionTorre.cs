using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RotacionTorre : MonoBehaviour, IDataPersistence
{
    [SerializeField] Quaternion rotation;
    [SerializeField] int caraTorre;

    CharacterControllerNonUnity characterController;
    public void LoadData(GameData data)
    {
        //caraTorre = data.currentFace;
    }
    public void SaveData(GameData data)
    {
    }
    private void Update()
    {
        caraTorre = characterController.currentFace;
    }
    private void Start()
    {
        characterController = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();

        if (characterController.currentFace == 1)
        {
            transform.rotation = Quaternion.Euler(0,0,0);
        }
        else if (characterController.currentFace == 2)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (characterController.currentFace == 3)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (characterController.currentFace == 4)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
    }
    
}