using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SavingScript.instance.hasLoaded)
        {
            transform.localRotation = SavingScript.instance.activeSave.TowerPosition;
        }
        else
        {
            SavingScript.instance.activeSave.TowerPosition = transform.localRotation;
            SavingScript.instance.activeSave.CurrentLevel = SceneManager.GetActiveScene().buildIndex;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SavingScript.instance.activeSave.TowerPosition = transform.localRotation;
        SavingScript.instance.activeSave.CurrentLevel = SceneManager.GetActiveScene().buildIndex;
    }
}
