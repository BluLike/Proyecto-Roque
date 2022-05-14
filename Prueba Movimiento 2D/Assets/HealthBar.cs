using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Renderer shader;
    public float health;
    public float amogus = 0.001f;
    //public float newHealth;
    // Start is called before the first frame update
    void Start()
    {
        var characterControllerNonUnity = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();
        shader = GetComponent<Renderer>();
        health = characterControllerNonUnity.life;
    }

    // Update is called once per frame
    void Update()
    {
        var characterControllerNonUnity = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();

        //while (health > characterControllerNonUnity.life)
        //{
            //tartCoroutine(Decrease());
        //}

        //while (health < characterControllerNonUnity.life)
        //{
            //StartCoroutine(Increase());
        //}
        shader.sharedMaterial.SetFloat("_Progress", (characterControllerNonUnity.life / 100f));
    }
    IEnumerator Decrease()
    {
        yield return new WaitForSeconds(0.5f);
        float a = health - 1f;
        health = a;
    }
    IEnumerator Increase()
    {
        yield return new WaitForSeconds(0.5f);
        float b = health + 1f;
        health = b;
    }
}
    
