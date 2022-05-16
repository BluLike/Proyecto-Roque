using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : MonoBehaviour
{
	public Vector3 direction;
	public bool hasHit = false;
	public float speed = 1.5f;
	public float Dmg = 35;
	private Animator animator;
	
	
	

    // Start is called before the first frame update
    void Start()
    {
	    animator = GetComponent<Animator>();
	    
	    

    }

    // Update is called once per frame
    void FixedUpdate()
    {
	    Physics.IgnoreLayerCollision(12,9); 
	    Physics.IgnoreLayerCollision(12,10);
		if ( !hasHit)
			GetComponent<Rigidbody>().velocity = direction * speed;
		
		Vector3 theScale = transform.localScale;
		theScale.x = direction.x*-1;
		transform.localScale = theScale;
		
		if (hasHit)
			GetComponent<Rigidbody>().velocity = direction * 0f;
		
		
    }

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			collision.gameObject.GetComponent<CharacterControllerNonUnity>().ApplyDamage(Dmg, transform.position);
			hasHit = true;
			StartCoroutine(ProjectileImpact());


		}
		else if (collision.gameObject.layer == 3 )
		{
			StartCoroutine(ProjectileImpact());
		}
	}
	public void EndProjectileLifetime()
	{
		StartCoroutine(ProjectileLimitDistance());
	}

	IEnumerator ProjectileLimitDistance()
	{
		yield return new WaitForSeconds(8f);
		Destroy(gameObject);
	}

	IEnumerator ProjectileImpact()
	{
		animator.Play("FireBall-Impact");
		
		gameObject.layer = 10;
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
	}
}
