using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthBar_smooth : MonoBehaviour
{
    // Start is called before the first frame update
    private float maxHP = 100, currHP, currHPSLow;
    public GameObject HealthSphere;
    
    private Renderer sphereRender;

    private Attack player;
    void Start()
    {
        currHP = maxHP;
        currHPSLow = maxHP;
        player = GetComponent<Attack>();
        sphereRender = HealthSphere.GetComponent<Renderer>();
        
    }

    private float t;
    private float a;
    
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
