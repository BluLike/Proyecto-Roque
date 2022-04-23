using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public CharacterControllerNonUnity characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterControllerNonUnity>();
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterControllerNonUnity>().ApplyDamage(10f, transform.position);
        }
    }
}
