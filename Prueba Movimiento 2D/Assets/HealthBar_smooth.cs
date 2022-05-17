using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthBar_smooth : MonoBehaviour
{
    // Start is called before the first frame update
    private float maxHP = 100, currHP, currHPSLow,AttackCooldown;
    public GameObject HealthSphere;
    public GameObject AttackCooldownSphere;
    private Renderer sphereRender;
    private Renderer AttackSphereRender;
    private Attack player;
    void Start()
    {
        currHP = maxHP;
        currHPSLow = maxHP;
        player = GetComponent<Attack>();
        sphereRender = HealthSphere.GetComponent<Renderer>();
        AttackSphereRender = AttackCooldownSphere.GetComponent<Renderer>();
        AttackCooldown = 100f;



    }

    private float t;
    private float a;
    
    // Update is called once per frame
    void Update()
    {
        if (player.canAttack == false)
        {
            AttackCooldown = 0f;
            AttackCooldown = Mathf.Lerp(0, 100, a);
            a += 1.0f * Time.deltaTime;
        }
        if (currHPSLow != currHP)
        {
            currHPSLow = Mathf.Lerp(currHPSLow, currHP, t);
            t += 1.0f * Time.deltaTime;
        }
        sphereRender.material.SetFloat("_Progress", currHPSLow*0.01f);
        AttackSphereRender.material.SetFloat("_Progress", AttackCooldown*0.01f);
    }

    public void loseHP(float damage)
    {
        currHP -= damage;
        t = 0;
    }
}
