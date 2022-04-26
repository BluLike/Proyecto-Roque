using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : MonoBehaviour
{
	public Vector3 direction;
	public bool hasHit = false;
	public float speed = 10f;
	

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
		if (collision.gameObject.tag == "Enemy")
		{
			collision.gameObject.SendMessage("ApplyDamage", Mathf.Sign(direction.x) * 2f);
			Destroy(gameObject);
		}
		else if (collision.gameObject.layer == 3 )
		{
			Destroy(gameObject);
		}
	}
}
