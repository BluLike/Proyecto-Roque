using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDisableFunction : MonoBehaviour
{
    //A simple function to disable/enable objects in animation

    public GameObject gameObject;
    public bool enable = true;
    void Update()
    {
        if (enable == false) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }
}
