using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterController>().ApplyDamage(10f, transform.position);
        }
    }
}
