using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TankEnemy : MonoBehaviour {

	[Header("Cara en la que se encuenrtra el enemigo:")]
	[SerializeField, Range(1, 4)] int enemyFace;
	
	public float life = 75;
	public int coins = 10;
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
	[SerializeField] private Transform m_GroundCheck;
	[SerializeField] private Transform m_GroundCheckFrontEnemy;
	[SerializeField] private LayerMask m_WhatIsGround;
	[SerializeField] private float Dmg;
	[SerializeField] private float speed;
	private bool distanceCheck;
	private float distance;
	private CharacterControllerNonUnity player;
	

	private bool facingRight = false;
	private bool m_Grounded;
	public bool m_GroundedFront;
	public bool trigger; 
	public bool isDead = false;
	public bool isAttacking = false;
	public bool canAttack = true;
	
	

	public bool isInvincible = false;
	private bool isHitted = false;

	//Animation states
	private const string IDLE = "Idle";
	private const string WALK = "Walk";
	private const string ATTACK1 = "Attack1";
	private const string ATTACK2 = "Attack2";
	private const string ATTACKTOIDLE = "AttackToIdle";
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
		playerTransform = GameObject.Find("DrawCharacter").GetComponent<Transform>();
		player = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();
		animator = GetComponent<Animator>();
		canAttack = true;
		


	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (player.currentFace != enemyFace) return;
		Physics.IgnoreLayerCollision(9, 14, true);
		distance = Vector3.Distance(playerTransform.position, transform.position);
		spriteRenderer = GetComponent<SpriteRenderer>();
		groundLayerMask = LayerMask.GetMask("Ground");

		if (life <= 0 && isDead == false) {
			ChangeAnimationState(DEATH);
			StartCoroutine(DestroyEnemy());
			isDead = true;
		}
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		m_GroundedFront = false;
		
		Collider[] FrontColliders = Physics.OverlapSphere(m_GroundCheckFrontEnemy.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < FrontColliders.Length; i++)
		{
			if (FrontColliders[i].gameObject != gameObject)
			{
				m_GroundedFront = true;
			}
		}

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

		if (distance <= 2.3f)
		{
			distanceCheck = true;
		}
		if (distance > 2.3f)
		{
			distanceCheck = false;
		}

		
		
		if (distanceCheck && isAttacking == false && !isDead && transform.position.x<playerTransform.position.x && facingRight && canAttack && !isHitted)
		{
			StartCoroutine(StartAtacking());
		}
		
		if (distanceCheck && isAttacking == false && !isDead && transform.position.x>playerTransform.position.x && !facingRight && canAttack && !isHitted)
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
			ChangeAnimationState(WALK);
			transform.position = Vector3.MoveTowards (transform.position, new Vector3(player.transform.position.x, transform.position.y,player.transform.position.z), speed * Time.deltaTime);
		}
		else if (m_GroundedFront == false && trigger == true && isAttacking == false && distanceCheck == false && isDead == false && isHitted == false && !isDead)
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

	public void ApplyDamage(float damage) {
		if (!isInvincible && life > 0 && !isDead) 
		{
			
			
				ChangeAnimationState(HURT);
                float direction = damage / Mathf.Abs(damage);
                damage = Mathf.Abs(damage);
                transform.GetComponent<Animator>().SetBool("Hit", true);
                life -= damage;
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(direction * 400f, 150f));
                StartCoroutine(HitTime());

			
			
		}
	}

	void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.tag == "Player" && life > 0)
		{
			collision.gameObject.GetComponent<CharacterControllerNonUnity>().ApplyDamage(Dmg/2, transform.position);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player" && life > 0)
			trigger = true;
		
		
	}

	IEnumerator StartAtacking()
	{
		isAttacking = true;
		
		ChangeAnimationState(ATTACK1);
		
		if (isHitted || isDead)
		{
			yield break;
		}

		
		yield return new WaitForSeconds(0.4f);
		
		if (distanceCheck && transform.position.x<playerTransform.position.x && facingRight && !isHitted)
		{
			player.ApplyDamage(Dmg,transform.position/2);

		}
		
		if (distanceCheck && transform.position.x>playerTransform.position.x && !facingRight && !isHitted)
		{
			player.ApplyDamage(Dmg,transform.position);

		}

		yield return new WaitForSeconds(0.4f);
		
		if (distanceCheck && transform.position.x<playerTransform.position.x && facingRight && !isDead && isHitted == false)
		{
			StartCoroutine(ContinueAtacking());

		}
		
		if (distanceCheck && transform.position.x>playerTransform.position.x && !facingRight && !isDead && isHitted == false)
		{
			StartCoroutine(ContinueAtacking());

		}
		
		
		else if (!isDead)
		{
			ChangeAnimationState(ATTACKTOIDLE);
			yield return new WaitForSeconds(0.3f);
			ChangeAnimationState(IDLE);	
			isAttacking = false;
			StartCoroutine(AttackCooldown());
		}
		

	}
	
	IEnumerator ContinueAtacking()
	{
		isAttacking = true;
		
		if (isHitted || isDead)
		{
			yield break;
		}
		ChangeAnimationState(ATTACK2);
		yield return new WaitForSeconds(0.4f);
		
		if (distanceCheck && !isHitted)
		{
			
			player.ApplyDamage(Dmg,transform.position);

		}
		
		if (distanceCheck && !isHitted)
		{
			player.ApplyDamage(Dmg,transform.position);

		}
		
		yield return new WaitForSeconds(0.7f);
		if (!isDead)
			ChangeAnimationState(IDLE);	
		
		isAttacking = false;
		StartCoroutine(AttackCooldown());
		

	}

	IEnumerator HitTime()
	{
		isHitted = true;
		isInvincible = true;
		yield return new WaitForSeconds(0.3f);
		isHitted = false;
		isInvincible = false;
		if (isDead == false)
			ChangeAnimationState(IDLE);

	}

	IEnumerator DestroyEnemy()
	{
		
		gameObject.layer = 10;
		player.AddCoins(coins);
		yield return new WaitForSeconds(0.25f);
		rb.velocity = new Vector2(0, rb.velocity.y);
		yield return new WaitForSeconds(3f);
		Destroy(gameObject);
	}
	
	IEnumerator AttackCooldown()
	{
		canAttack=false;
		yield return new WaitForSeconds(1.5f);
		canAttack = true;
		
	}
	


}
