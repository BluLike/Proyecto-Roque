using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Renderer shader;
    //public float oldHealth;
    //public float newHealth;
    // Start is called before the first frame update
    void Awake()
    {
        var characterControllerNonUnity = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();
        shader = GetComponent<Renderer>();
        //oldHealth = characterControllerNonUnity.life;
        //newHealth = oldHealth;
    }

    // Update is called once per frame
    void Update()
    {
        var characterControllerNonUnity = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();
        //if (oldHealth > characterControllerNonUnity.life)
        //{
        //    LoopDecrease();
        //}
        //if (oldHealth < characterControllerNonUnity.life)
        //{
        //    LoopIncrease();
        //}
        shader.sharedMaterial.SetFloat("_Progress", (characterControllerNonUnity.life / 100));
    }
    //void LoopDecrease()
   // {
   //     var characterControllerNonUnity = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();
   //     for (float i = 0; oldHealth > characterControllerNonUnity.life; i++)
   //     {
   //         newHealth = Mathf.Lerp(oldHealth, characterControllerNonUnity.life, i / 10);
   //     }
   //     oldHealth = newHealth;
   // }
   // void LoopIncrease()
   // {
   //     var characterControllerNonUnity = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();
   //     for (float i = 0; oldHealth < characterControllerNonUnity.life; i++)
   //     {
   //         newHealth = Mathf.Lerp(oldHealth, characterControllerNonUnity.life, i/10);
   //     }
   //     oldHealth = newHealth;
   // }
}
