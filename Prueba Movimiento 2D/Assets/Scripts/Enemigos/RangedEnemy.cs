using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangedEnemy : MonoBehaviour {

	[Header("Cara en la que se encuenrtra el enemigo:")]
	[SerializeField, Range(1, 4)] int enemyFace;

	public float life = 75;
	public int coins = 10;
	public GameObject FireBall;
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
	public bool canShoot = true;
	public bool isDead;
	private CharacterControllerNonUnity player;
	
	
	public GameObject proyectile;
	public ThrowableWeapon Weapon;
	

	private bool facingRight = false;
	private bool m_Grounded;

	

	public bool isInvincible = false;
	private bool isHitted = false;
	
	
	//Animation states
	private const string IDLE = "Idle";
	private const string SPELLCAST = "SpellCast";
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
		player = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();
		fallCheck = transform.Find("FallCheck");
		wallCheck = transform.Find("WallCheck");
		rb = GetComponent<Rigidbody>();
		boxCollider = GetComponent<BoxCollider>();
		playerTransform = GameObject.Find("DrawCharacter").GetComponent<Transform>();
		animator = GetComponent<Animator>();
		isDead = false;

	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (player.currentFace != enemyFace) return;
		Physics.IgnoreLayerCollision(9, 14, true);
		spriteRenderer = GetComponent<SpriteRenderer>();
		groundLayerMask = LayerMask.GetMask("Ground");

		if (life <= 0 && !isDead) 
		{
			isDead = true;
			ChangeAnimationState(DEATH);
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
		if (!isInvincible && life > 0) 
		{
			ChangeAnimationState(HURT);
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
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
		if(other.gameObject.tag=="Player" && life > 0)
		{
			
			if (!isHitted && !isDead)
			{
				ChangeAnimationState(IDLE);
			}
		}
		
	}

	private void OnTriggerStay(Collider other)
	{
		if (player.currentFace != enemyFace) return;
		if (other.gameObject.tag == "Player" && life > 0 && isHitted == false)
		{
			
			if (canShoot && !isHitted)
			{
				ChangeAnimationState(SPELLCAST);
				StartCoroutine(SpellCast());
				StartCoroutine(FireBallCooldown());
			}
		}
	}

	IEnumerator HitTime()
	{
		isHitted = true;
		isInvincible = true;
		yield return new WaitForSeconds(0.3f);
		isHitted = false;
		isInvincible = false;
		if (!isDead)
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
	
	IEnumerator SpellCast()
	{
		Vector3 position;
		yield return new WaitForSeconds(0.4f);
		if (transform.localScale.x < 0)
		{
			position = transform.position + new Vector3(transform.localScale.x-0.5f * 0.7f, 0, 0);
		}
		else
		{
			position = transform.position + new Vector3(transform.localScale.x+0.5f * 0.7f, 0, 0);
		}
		proyectile = Instantiate(FireBall, position, Quaternion.identity) as GameObject;
		Vector3 direction = new Vector3(transform.localScale.x, 0, 0);
		proyectile.GetComponent<ThrowableWeapon>().direction = direction; 
		proyectile.name = "FireBall";
		Weapon = GameObject.Find("FireBall").GetComponent<ThrowableWeapon>();
		Weapon.EndProjectileLifetime();
		
	}

	IEnumerator FireBallCooldown()
	{
		canShoot = false;
		yield return new WaitForSeconds(1.5f);
		canShoot = true;
		if(!isDead)
			ChangeAnimationState(IDLE);
	}
	

}