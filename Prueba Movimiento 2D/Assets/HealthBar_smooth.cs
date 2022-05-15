using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthBar_smooth : MonoBehaviour
{
    // Start is called before the first frame update
    private float maxHP = 100, currHP, currHPSLow;
    public GameObject HealthSphere;
    private Renderer sphereRender;
    private CharacterControllerNonUnity player;
    void Start()
    {
        currHP = maxHP;
        currHPSLow = maxHP;
        sphereRender = HealthSphere.GetComponent<Renderer>();
        

    }

    private float t = 0;
    
    // Update is called once per frame
    void Update()
    {
        if (currHPSLow != currHP)
        {
            currHPSLow = Mathf.Lerp(currHPSLow, currHP, t);
            t += 1.0f * Time.deltaTime;
        }
        sphereRender.material.SetFloat("_Progress", currHPSLow*0.01f);
    }

    public void loseHP(float damage)
    {
        currHP -= damage;
        t = 0;
    }
}
