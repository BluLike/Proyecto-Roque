using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine.UIElements;

public class Trader : MonoBehaviour
{
	
	
	[Header("El resto de cosas XD")]
	public float life = 75;
	public int coins = 3;
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
	
	private bool distanceCheck;
	private float distance;
	private CharacterControllerNonUnity player;
												            

	private bool facingRight = false;
	private bool m_Grounded;
	
	public bool trigger = false ;
	

	public bool isInvincible = false;
	private bool isHitted = false;

	//Animation states
	private const string IDLE = "Idle";
	
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
		animator = GetComponent<Animator>();


	}
	
	
	void FixedUpdate ()
	{
		Physics.IgnoreLayerCollision(9, 14, true);
		distance = Vector3.Distance(playerTransform.position, transform.position);
		spriteRenderer = GetComponent<SpriteRenderer>();
		groundLayerMask = LayerMask.GetMask("Ground");
    
		
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		
            
		
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
    
		
    
		if (distance <= 2f)
		{
			distanceCheck = true;
		}
		if (distance > 2f)
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
		else
			ChangeAnimationState(IDLE);
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

	

	

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player" && life > 0)
			trigger = true;

	}
	

}
