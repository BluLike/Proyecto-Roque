using System;
using UnityEngine;
using System.Collections;

public class RangedEnemy : MonoBehaviour {

	public float life = 75;
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
	
	public GameObject proyectile;
	public ThrowableWeapon Weapon;
	

	private bool facingRight = false;
	private bool m_Grounded;

	

	public bool isInvincible = false;
	private bool isHitted = false;

	void Awake () {
		fallCheck = transform.Find("FallCheck");
		wallCheck = transform.Find("WallCheck");
		rb = GetComponent<Rigidbody>();
		boxCollider = GetComponent<BoxCollider>();
		playerTransform = GameObject.Find("DrawCharacter").GetComponent<Transform>();
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
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

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player" && life > 0)
		{
			if (canShoot)
			{
				proyectile = Instantiate(FireBall, transform.position + new Vector3(transform.localScale.x * 0.7f,0,0), Quaternion.identity) as GameObject;
				Vector3 direction = new Vector3(transform.localScale.x, 0, 0);
				proyectile.GetComponent<ThrowableWeapon>().direction = direction; 
				proyectile.name = "FireBall";
				Weapon = GameObject.Find("FireBall").GetComponent<ThrowableWeapon>();
				Weapon.EndProjectileLifetime();
				StartCoroutine(FireBallCooldown());
			}
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

	IEnumerator FireBallCooldown()
	{
		canShoot = false;
		yield return new WaitForSeconds(1.1f);
		canShoot = true;
	}
	

}
