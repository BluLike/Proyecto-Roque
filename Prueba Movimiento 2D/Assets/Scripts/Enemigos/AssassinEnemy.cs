using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssassinEnemy : MonoBehaviour {

	public float life = 75;
	private bool isPlat;
	private bool isObstacle;
	private Transform fallCheck;
	private Transform wallCheck;
	public LayerMask turnLayerMask;
	public LayerMask groundLayerMask;
	public SpriteRenderer spriteRenderer;
	const float k_GroundedRadius = .2f;
	private Rigidbody rb;
	private Transform playerTransform;
	public BoxCollider boxCollider;
	[SerializeField] private Transform m_GroundCheck;
	[SerializeField] private LayerMask m_WhatIsGround;
	[SerializeField] private float Dmg;
	private bool distanceCheck;
	private float distance;
	private float timeLeft = 3.0f;
	private CharacterControllerNonUnity player;
	

	private bool facingRight = false;
	private bool m_Grounded;
	

	public bool isInvincible = false;
	private bool isHitted = false;

	//Animation states
	private const string IDLE = "Idle";
	private const string RUN = "Run";
	private const string ATTACK = "Attack";
	private const string VANISH = "Vanish";
	private const string ARISE = "Arise";
	private const string HURT = "Hurt";
	private const string DEATH = "Death";

	
	//Animator parameters
	
	private Animator animator;
	private string currentState;

	void ChangeAnimationState(string newState)
	{
		//evita que se quede en bucle la animación
		if (currentState == newState) return;
		//hace la animación 
		animator.Play(newState);
		//asignar nuevo valor de la animación
		currentState = newState;
	}
	
	void Awake () {
		fallCheck = transform.Find("FallCheck");
		wallCheck = transform.Find("WallCheck");
		rb = GetComponent<Rigidbody>();
		boxCollider = GetComponent<BoxCollider>();
		playerTransform = GameObject.Find("DrawCharacter").GetComponent<Transform>();
		player = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();


	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		distance = Vector3.Distance(playerTransform.position, transform.position);
		spriteRenderer = GetComponent<SpriteRenderer>();
		groundLayerMask = LayerMask.GetMask("Ground");

		if (life <= 0) {
			transform.GetComponent<Animator>().SetBool("IsDead", true);
			StartCoroutine(DestroyEnemy());
		}
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider[] colliders = Physics.OverlapSphere(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				m_Grounded = true;
			if (!wasGrounded)
			{
			}

		}

		isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << groundLayerMask);
		isObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);

		if (!isHitted && life > 0 && Mathf.Abs(rb.velocity.y) < 0.5f)
		{
			if (m_Grounded && !isObstacle && !isHitted)
			{
				if (facingRight)
				{
					//rb.velocity = new Vector2(-speed, rb.velocity.y);
				}
				else
				{
					//rb.velocity = new Vector2(speed, rb.velocity.y);
				}
			}
		}
		
		if (distance < 1.5f)
		{
			distanceCheck = true;
			StartAtacking();
		}
		if (distance > 1.5f)
		{
			distanceCheck = false;
		}


		if (transform.position.x>playerTransform.position.x)
		{
			if (facingRight)
			{
				FlipL();
			}
		}
		
		if (transform.position.x<playerTransform.position.x)
		{
			if (!facingRight)
			{
				FlipR();
			}
		}
	}


	private void StartAtacking()
	{
		timeLeft -= Time.deltaTime;
		
		if (timeLeft <= 0f && distanceCheck)
		{
			player.ApplyDamage(Dmg,transform.position);
			timeLeft = 3;

		}
		if (timeLeft <= 0f && !distanceCheck)
		{
			timeLeft = 3;
		}
		

	}
	private void FlipR()
	{
		facingRight = true;
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x = 1;
		transform.localScale = theScale;
	}
	private void FlipL()
	{
		facingRight = false;
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x = -1;
		transform.localScale = theScale;
	}

	public void ApplyDamage(float damage) {
		if (!isInvincible) 
		{
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
			transform.GetComponent<Animator>().SetBool("Hit", true);
			life -= damage;
			rb.velocity = Vector2.zero;
			rb.AddForce(new Vector2(direction * 400f, 100f));
			StartCoroutine(HitTime());

		}
	}

	void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.tag == "Player" && life > 0)
		{
			collision.gameObject.GetComponent<CharacterControllerNonUnity>().ApplyDamage(Dmg, transform.position);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player" && life > 0)
		{
			StartCoroutine(AssassinTp(other));
		}
		
	}

	

	IEnumerator HitTime()
	{
		isHitted = true;
		isInvincible = true;
		yield return new WaitForSeconds(0.1f);
		isHitted = false;
		isInvincible = false;
		spriteRenderer.color = Color.white;

	}

	IEnumerator DestroyEnemy()
	{
		
		gameObject.layer = 10;
		yield return new WaitForSeconds(0.25f);
		rb.velocity = new Vector2(0, rb.velocity.y);
		yield return new WaitForSeconds(3f);
		Destroy(gameObject);
	}

	IEnumerator AssassinTp(Collider other)
	{
		yield return new WaitUntil( () => other.gameObject.GetComponent<CharacterControllerNonUnity>().m_Grounded == true);
		
		if (other.gameObject.tag == "Player" && life > 0)
		{
			if (other.gameObject.GetComponent<CharacterControllerNonUnity>().m_TpGroundedBack == true && other.gameObject.GetComponent<CharacterControllerNonUnity>().m_TpGroundedFront == false)
			{
				transform.position = new Vector3(other.gameObject.GetComponent<CharacterControllerNonUnity>().m_TpGroundCheckBack.transform.position.x, playerTransform.position.y, playerTransform.position.z);
			}
			
			if (other.gameObject.GetComponent<CharacterControllerNonUnity>().m_TpGroundedBack == false && other.gameObject.GetComponent<CharacterControllerNonUnity>().m_TpGroundedFront == true)
			{
				transform.position = new Vector3(other.gameObject.GetComponent<CharacterControllerNonUnity>().m_TpGroundCheckFront.transform.position.x, playerTransform.position.y, playerTransform.position.z);
			}
			
			else if(other.gameObject.GetComponent<CharacterControllerNonUnity>().m_TpGroundedBack == true && other.gameObject.GetComponent<CharacterControllerNonUnity>().m_TpGroundedFront == true)
			{
				if (transform.localScale.x ==-1)
				{
					transform.position = new Vector3(playerTransform.position.x-1.3f, playerTransform.position.y, playerTransform.position.z);
				}
				
				if (transform.localScale.x == 1)
				{
					transform.position = new Vector3(playerTransform.position.x+1.3f, playerTransform.position.y, playerTransform.position.z);
				}	
			}
			
		}
		
	}


}
