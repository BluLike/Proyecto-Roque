using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
	public Vector3 direction;
	public bool hasHit = false;
	public float speed = 20f;
	public bool ganchito;
	public CharacterControllerNonUnity characterController;

	// Start is called before the first frame update
	void Start()
	{
		characterController = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!hasHit)
        {
			GetComponent<Rigidbody>().velocity = direction * speed;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			characterController.ApplyGrapleForce();
			Destroy(gameObject);
		}
		else if (collision.gameObject.layer == 3)
		{
			Destroy(gameObject);
		}
	}
}