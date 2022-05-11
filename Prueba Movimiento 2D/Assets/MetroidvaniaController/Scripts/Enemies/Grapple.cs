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
	private bool m_FacingRight;
	

	// Start is called before the first frame update
	void Start()
	{
		characterController = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();
		
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!hasHit) GetComponent<Rigidbody>().velocity = direction * speed;

		if (characterController.transform.localScale.x < 0 &&(!m_FacingRight))
		{
			Flip();
		}

	}

	private void Flip()
	{
		m_FacingRight = !m_FacingRight;
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
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