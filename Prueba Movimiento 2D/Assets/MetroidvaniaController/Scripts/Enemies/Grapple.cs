using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
	public Vector3 direction;
	public bool hasHit = false;
	public float speed = 20f;
	public CharacterControllerNonUnity characterController;
	

	// Start is called before the first frame update
	void Start()
	{
		characterController = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();
		
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!hasHit) GetComponent<Rigidbody>().velocity = direction * speed;

		Vector3 theScale = characterController.transform.localScale;
		transform.localScale = theScale;

	}

	

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Anilla")
		{
			characterController.ApplyGrapleForce();
			Destroy(gameObject);
		}
	}
	

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 3)
		{
			Destroy(gameObject);
		}
	}

	public void DestroyGrapple()
	{
		StartCoroutine(GrappleLimitDistance());
	}

	IEnumerator GrappleLimitDistance()
	{
		yield return new WaitForSeconds(0.25f);
		Destroy(gameObject);
	}
}