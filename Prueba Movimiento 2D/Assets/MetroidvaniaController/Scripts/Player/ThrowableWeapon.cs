using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : MonoBehaviour
{
	public Vector3 direction;
	public bool hasHit = false;
	public float speed = 1.5f;
	public float Dmg = 35;
	
	

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if ( !hasHit)
			GetComponent<Rigidbody>().velocity = direction * speed;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			collision.gameObject.GetComponent<CharacterControllerNonUnity>().ApplyDamage(Dmg, transform.position);
			Destroy(gameObject);
		}
		else if (collision.gameObject.layer == 3 )
		{
			
			Destroy(gameObject);
		}
	}
	public void DestroyProyectile()
	{
		StartCoroutine(ProyectileLimitDistance());
	}

	IEnumerator ProyectileLimitDistance()
	{
		yield return new WaitForSeconds(8f);
		Destroy(gameObject);
	}
}
