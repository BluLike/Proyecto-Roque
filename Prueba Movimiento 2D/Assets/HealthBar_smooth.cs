using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthBar_smooth : MonoBehaviour, IDataPersistence
{
    // Start is called before the first frame update
    [SerializeField] private float maxHP = 100, currHP, currHPSLow;
    float loadHP;
    public GameObject HealthSphere;
    
    private Renderer sphereRender;

    [SerializeField] float materialFloat;

    public void LoadData(GameData data)
    {
        this.currHPSLow = data.currHPSLow;
        loadHP = this.currHPSLow;
    }

    public void SaveData(GameData data)
    {
    }

    private float t;
    private float a;

    void Start()
    {
        currHP = loadHP;
        currHPSLow = loadHP;
        sphereRender = HealthSphere.GetComponent<Renderer>();
        sphereRender.material.SetFloat("_Progress", currHPSLow * 0.01f);
    }
    // Update is called once per frame
    void Update()
    {
        if (currHP > maxHP)
            currHP = maxHP;
        if (currHPSLow != currHP)
        {
            currHPSLow = Mathf.Lerp(currHPSLow, currHP, t);
            t += 1.0f * Time.deltaTime;
        }
       sphereRender.material.SetFloat("_Progress", currHPSLow * 0.01f);
       materialFloat = sphereRender.material.GetFloat("_Progress");



    }

    public void loseHP(float damage)
    {
        currHP -= damage;
        t = 0;
    }
    public void healHP(float heal)
    {
        currHP += heal;
        t = 0;
    }
}
