using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine.UIElements;

public class AssassinEnemy : MonoBehaviour
{
	[Header("Cara en la que se encuenrtra el enemigo:")]
	[SerializeField, Range(1, 4)] int enemyFace;
	
	[Header("El resto de cosas XD")]
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
	[SerializeField] private Transform m_GroundCheckFrontEnemy;
	[SerializeField] private LayerMask m_WhatIsGround;
	[SerializeField] private float Dmg;
	[SerializeField] private float speed = 50;
	public bool m_GroundedFront;
	private bool distanceCheck;
	private float distance;
	private CharacterControllerNonUnity player;
												            

	private bool facingRight = false;
	private bool m_Grounded;
	public bool isDead = false;
	public bool isAttacking = false;
	public bool isVanished = false;
	public bool trigger = false ;
	

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
		animator = GetComponent<Animator>();


	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (player.currentFace != enemyFace) return;
		Physics.IgnoreLayerCollision(9, 14, true);
		distance = Vector3.Distance(playerTransform.position, transform.position);
		spriteRenderer = GetComponent<SpriteRenderer>();
		groundLayerMask = LayerMask.GetMask("Ground");
    
		if (life <= 0 && !isDead) {
			ChangeAnimationState(DEATH);
			StartCoroutine(DestroyEnemy());
			isDead = true;
		}
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		m_GroundedFront = false;
            
    
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
    
            
    
		Collider[] FrontColliders = Physics.OverlapSphere(m_GroundCheckFrontEnemy.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < FrontColliders.Length; i++)
		{
			if (FrontColliders[i].gameObject != gameObject)
			{
				m_GroundedFront = true;
			}
		}
    
		if (distance <= 1.5f)
		{
			distanceCheck = true;
		}
		if (distance > 1.5f)
		{
			distanceCheck = false;
		}
            
            
		if (distanceCheck && isAttacking == false && !isDead && transform.position.x<playerTransform.position.x && facingRight)
		{
			StartCoroutine(StartAtacking());
		}
            
		if (distanceCheck && isAttacking == false && !isDead && transform.position.x>playerTransform.position.x && !facingRight)
		{
			StartCoroutine(StartAtacking());
		}
    
		if (transform.position.x>playerTransform.position.x && !isDead && !isAttacking)
		{
			if (facingRight)
			{
				FlipL();
			}
		}
            
		if (transform.position.x<playerTransform.position.x && !isDead && !isAttacking)
		{
			if (!facingRight)
			{
				FlipR();
			}
		}
    
		if (transform.position.x!=playerTransform.position.x && m_GroundedFront == true && trigger == true && isAttacking == false && distanceCheck == false && isHitted == false && !isDead)
		{
			ChangeAnimationState(RUN);
			transform.position = Vector3.MoveTowards (transform.position, new Vector3(player.transform.position.x, transform.position.y,player.transform.position.z), speed * Time.deltaTime);
		}
		else if (m_GroundedFront == false && trigger == true && isAttacking == false && distanceCheck == false && isVanished == false && isDead == false && isHitted == false)
			ChangeAnimationState(IDLE);
	}

	
	private void FlipR()
	{
		if (player.currentFace != enemyFace) return;
		facingRight = true;
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x = 1;
		transform.localScale = theScale;

	}
	private void FlipL()
	{
		if (player.currentFace != enemyFace) return;
		facingRight = false;
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x = -1;
		transform.localScale = theScale;

	}

	public void ApplyDamage(float damage)
	{
		if (!isInvincible && life > 0 && isDead == false) 
		{
            	
			ChangeAnimationState(HURT);
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

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player" && life > 0)
			trigger = true;

	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player" && life > 0)
		{
			trigger = false;
			StartCoroutine(AssassinTp(other));
		}
		
	}

	IEnumerator StartAtacking()
	{
		isAttacking = true;
		
		ChangeAnimationState(ATTACK);
		if  (isHitted || isDead)
		{
			yield break;
		} 
			
		
		yield return new WaitForSeconds(0.65f);
		
		if (distanceCheck && transform.position.x<playerTransform.position.x && facingRight && !isHitted && !isDead)
		{
			player.ApplyDamage(Dmg,transform.position/2);

		}
		
		if (distanceCheck && transform.position.x>playerTransform.position.x && !facingRight && isHitted == false && isDead == false)
		{
			player.ApplyDamage(Dmg,transform.position/2);

		}

		yield return new WaitForSeconds(0.55f);
		if (!isDead)
			ChangeAnimationState(IDLE);
		isAttacking = false;


	}

	IEnumerator HitTime()
	{
		isHitted = true;
		
		isInvincible = true;
		yield return new WaitForSeconds(0.1f);
		isHitted = false;
		isInvincible = false;
		if (!isDead)
			ChangeAnimationState(IDLE);
		

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
		isVanished = true;
		yield return new WaitUntil( () => isAttacking == false);
		yield return new WaitUntil( () => other.gameObject.GetComponent<CharacterControllerNonUnity>().m_Grounded == true);
		ChangeAnimationState(VANISH);
		yield return new WaitForSeconds(0.8f);
		if (other.gameObject.tag == "Player" && life > 0)
		{
			if (other.gameObject.GetComponent<CharacterControllerNonUnity>().m_TpGroundedBack == true && other.gameObject.GetComponent<CharacterControllerNonUnity>().m_TpGroundedFront == false)
			{
				transform.position = new Vector3(other.gameObject.GetComponent<CharacterControllerNonUnity>().m_TpGroundCheckBack.transform.position.x - 0.3f, playerTransform.position.y, playerTransform.position.z);
			}
			
			if (other.gameObject.GetComponent<CharacterControllerNonUnity>().m_TpGroundedBack == false && other.gameObject.GetComponent<CharacterControllerNonUnity>().m_TpGroundedFront == true)
			{
				transform.position = new Vector3(other.gameObject.GetComponent<CharacterControllerNonUnity>().m_TpGroundCheckFront.transform.position.x + 0.3f, playerTransform.position.y, playerTransform.position.z);
			}
			
			else if(other.gameObject.GetComponent<CharacterControllerNonUnity>().m_TpGroundedBack == true && other.gameObject.GetComponent<CharacterControllerNonUnity>().m_TpGroundedFront == true)
			{
				if (transform.localScale.x ==-1)
				{
					transform.position = new Vector3(other.gameObject.GetComponent<CharacterControllerNonUnity>().m_GroundCheck.transform.position.x-1f, playerTransform.position.y, playerTransform.position.z);
				}
				
				if (transform.localScale.x == 1)
				{
					transform.position = new Vector3(other.gameObject.GetComponent<CharacterControllerNonUnity>().m_GroundCheck.transform.position.x+1f, playerTransform.position.y, playerTransform.position.z);
				}	
			}
			
		}
		yield return new WaitForSeconds(0.2f);
		ChangeAnimationState(ARISE);
		yield return new WaitForSeconds(0.6f);
		if (!isDead)
			ChangeAnimationState(IDLE);
		isVanished = false;

	}


}
