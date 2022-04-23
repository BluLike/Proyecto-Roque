using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{

    [SerializeField] private float Dmg;

    
    
    void OnCollisionStay(Collision collision)
    {
        
        if (collision.gameObject.tag == "Player" )
        {
            collision.gameObject.GetComponent<CharacterControllerNonUnity>().ApplyFallDamage(Dmg);
        }
    }
}
