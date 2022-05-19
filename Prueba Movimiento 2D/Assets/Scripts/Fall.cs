using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{

    [SerializeField] private float Dmg;
    private Animator animator;
    private Rigidbody rb;

    
    
    void OnCollisionStay(Collision collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterControllerNonUnity>().ApplyFallDamage(Dmg);
        }
        
        if (collision.gameObject.tag == "Enemy" && collision.gameObject != null )
        {
            animator = collision.gameObject.GetComponent<Animator>();
            animator.Play("Death");
            rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;
            StartCoroutine(DestroyEnemy(collision));

        }
    }
    
    
    IEnumerator DestroyEnemy(Collision other)
    {
        
        Destroy(other.gameObject);
        yield return new WaitForSeconds(0f);
     
        
    }
}
