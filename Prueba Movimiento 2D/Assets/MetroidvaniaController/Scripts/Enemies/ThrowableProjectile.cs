using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableProjectile : MonoBehaviour
{
	public Vector2 direction;
	public bool hasHit = false;
	public float speed = 15f;
	public GameObject owner;

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
			collision.gameObject.GetComponent<CharacterControllerNonUnity>().ApplyDamage(2f, transform.position);
			Destroy(gameObject);
		}
		else if ( owner != null && collision.gameObject != owner && collision.gameObject.tag == "Enemy" )
		{
			collision.gameObject.SendMessage("ApplyDamage", Mathf.Sign(direction.x) * 2f);
			Destroy(gameObject);
		}
		else if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Player")
		{
			Destroy(gameObject);
		}
	}
}
