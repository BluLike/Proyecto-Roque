using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDisableFunction : MonoBehaviour
{
    //A simple function to disable/enable objects in animation

    public GameObject GGameObject;
    public bool enable = true;
    void Update()
    {
        if (enable == false) GGameObject.SetActive(false);
        else GGameObject.SetActive(true);
    }
}
