using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.SceneManagement;

public class CharacterControllerNonUnity : MonoBehaviour, IDataPersistence
{
	[SerializeField] private float m_JumpForce = 920f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] public Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] public Transform m_TpGroundCheckFront;					
	[SerializeField] public Transform m_TpGroundCheckBack;						
	[SerializeField] private Transform m_WallCheck;								//Posicion que controla si el personaje toca una pared
	[SerializeField] private int healValue = 30;
	[SerializeField] private int potionsNumber;
	[SerializeField] private int coins;
	
	public bool canHeal = true;
								

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	float k_wallCheckRadius = k_GroundedRadius * 2.3f;
	public bool m_Grounded;            // Whether or not the player is grounded.
	public bool m_TpGroundedFront;            // Whether or not the player is grounded.
	public bool m_TpGroundedBack;            // Whether or not the player is grounded.
	private Rigidbody m_Rigidbody;
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;
	private float limitFallSpeed = 90000f; // Limit fall speed

	public bool canDoubleJump = true; //If player can double jump
	[SerializeField] private float m_DashForce = 25f;
	[SerializeField] public float m_GrappleForce = 2000000f;
	private bool canDash = true;
	private bool isDashing = false; //If player is dashing
	private bool m_IsWall = false; //If there is a wall in front of the player
	private bool isWallSliding = false; //If player is sliding in a wall
	private bool oldWallSlidding = false; //If player is sliding in a wall in the previous frame
	private float prevVelocityX = 0f;
	private bool canCheck = false; //For check if player is wallsliding

	public float life; //Life of the player
	public bool invincible = false; //If player can die
	public bool canMove = true; //If player can move

	private Animator animator;
	public ParticleSystem particleJumpUp; //Trail particles
	public ParticleSystem particleJumpDown; //Explosion particles
	public ParticleSystem healParticle;	//particulas curación.

	private float jumpWallStartX = 0;
	private float jumpWallDistX = 0; //Distance between player and wall
	private bool limitVelOnWallJump = false; //For limit wall jump distance with low fps

	public SpriteRenderer spriteRenderer;
	public Color mColor;
	private HealthBar_smooth healthbar;
	public bool isGrapplePulling = false;
	public PlayerMovement player;
	public TextMeshPro potionIndicator;
	public AudioSource audioSource;
	public AudioClip audioClip;
	Scene m_Scene;
	string currentScene;

	public int currentFace = 1;

	[Header("Events")]
	[Space]

	public UnityEvent OnFallEvent;
	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public void LoadData(GameData data)
    {
		this.life = data.life;
		this.potionsNumber = data.potionsNumber;
		this.coins = data.coins;
		this.currentScene = data.currentScene;
		this.currentFace = data.currentFace;
    }

	public void SaveData(GameData data)
    {
		data.currHPSLow = this.life;
		data.life = this.life;
		data.potionsNumber = this.potionsNumber;
		data.coins = this.coins;
		data.currentScene = this.currentScene;
		data.currentFace = this.currentFace;
    }
    private void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		healthbar = GetComponent<HealthBar_smooth>();
		player = gameObject.GetComponent<PlayerMovement>();
		audioSource = GetComponent<AudioSource>();
		

		if (OnFallEvent == null)
			OnFallEvent = new UnityEvent();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}


	private void FixedUpdate()
	{
		Physics.IgnoreLayerCollision(8,14);
		Physics.IgnoreLayerCollision(8,10);
		Physics.IgnoreLayerCollision(8,11);
		Physics.IgnoreLayerCollision(8,13);
		Physics.IgnoreLayerCollision(9,9);
		m_Scene = SceneManager.GetActiveScene();
		currentScene = m_Scene.name;

		if (life > 100)
			life = 100;
		potionIndicator.text = Convert.ToString(potionsNumber);
		//heal function
		if (Input.GetKeyDown(KeyCode.F) && canHeal|| Input.GetMouseButtonDown(2) && canHeal)
		{
			if (life >= 100f || potionsNumber < 1)
				return;
			audioSource.PlayOneShot( audioClip, 0.5f);
			healParticle.Play();
			healthbar.healHP(healValue);
			life += healValue;
			potionsNumber--;

			StartCoroutine(HealCooldown());
		}
		
		if(!isDashing)
		{
			Physics.IgnoreLayerCollision(8,9, false);
			Physics.IgnoreLayerCollision(8,12, false);
			Color mColor = new Color(1, 1, 1, 1);
			spriteRenderer.color = mColor;
		}
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		m_TpGroundedBack = false;
		m_TpGroundedFront = false;
		if (wasGrounded)
        {
			canDash = true;
        }
		if (!wasGrounded)
        {
			canDash = false;
        }


		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider[] colliders = Physics.OverlapSphere(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				Physics.gravity = new Vector3(0, -28f, 0);
			}
				
			
			if (!wasGrounded )
			{
				OnLandEvent.Invoke();
				if (!m_IsWall && !isDashing) 
					particleJumpDown.Play();
				canDoubleJump = true;
				if (m_Rigidbody.velocity.y < 0f)
					limitVelOnWallJump = false;
			}
		}
		
		Collider[] TpFrontColliders = Physics.OverlapSphere(m_TpGroundCheckFront.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < TpFrontColliders.Length; i++)
		{
			if (TpFrontColliders[i].gameObject != gameObject)
			{
				m_TpGroundedFront = true;
			}
		}
		Collider[] TpBackColliders = Physics.OverlapSphere(m_TpGroundCheckBack.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < TpBackColliders.Length; i++)
		{
			if (TpBackColliders[i].gameObject != gameObject)
			{
				m_TpGroundedBack = true;
			}
		}

		m_IsWall = false;
		
		if (!m_Grounded)
		{
			OnFallEvent.Invoke();
			
			


			prevVelocityX = m_Rigidbody.velocity.x;
		}

		if (limitVelOnWallJump)
		{
			if (m_Rigidbody.velocity.y < -0.5f)
				limitVelOnWallJump = false;
			jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.localScale.x;
			if (jumpWallDistX < -0.5f && jumpWallDistX > -1f) 
			{
				canMove = true;
			}
			else if (jumpWallDistX < -1f && jumpWallDistX >= -2f) 
			{
				canMove = true;
				m_Rigidbody.velocity = new Vector2(10f * transform.localScale.x, m_Rigidbody.velocity.y);
			}
			else if (jumpWallDistX < -2f) 
			{
				limitVelOnWallJump = false;
				m_Rigidbody.velocity = new Vector2(0, m_Rigidbody.velocity.y);
			}
			else if (jumpWallDistX > 0) 
			{
				limitVelOnWallJump = false;
				m_Rigidbody.velocity = new Vector2(0, m_Rigidbody.velocity.y);
			}
		}
		
	}

	private void OnTriggerStay(Collider collisionInfo)
	{
		if (!m_Grounded)
		{
			OnFallEvent.Invoke();
			if (collisionInfo.gameObject.layer == 3 )
			{
				isDashing = false;
				m_IsWall = true;
				m_Grounded = false;
			}
		}
		
	}


	public void Move(float move, bool jump, bool dash)
	{
		if (canMove) {
			
			if (dash && canDash && !isWallSliding)
			{
				//m_Rigidbody2D.AddForce(new Vector2(transform.localScale.x * m_DashForce, 0f));
				StartCoroutine(DashCooldown());
			}
			// If crouching, check to see if the character can stand up
			if (isDashing)
			{
				StartCoroutine(MakeInvincible(0.4f));
				m_Rigidbody.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
				Physics.IgnoreLayerCollision(8,9, true);
				Physics.IgnoreLayerCollision(8,12, true);
				Color mColor = new Color(1, 1, 1, 0.3f);
				spriteRenderer.color = mColor;
			}
			//only control the player if grounded or airControl is turned on
			else if (m_Grounded || m_AirControl)
			{
				if (m_Rigidbody.velocity.y < -limitFallSpeed)
					m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, -100*100);
				// Move the character by finding the target velocity
				Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody.velocity.y);
				// And then smoothing it out and applying it to the character
				m_Rigidbody.velocity = Vector3.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !m_FacingRight && !isWallSliding)
				{
					// ... flip the player.
					Flip();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0 && m_FacingRight && !isWallSliding)
				{
					// ... flip the player.
					Flip();
				}
			}
			// If the player should jump...
			if (m_Grounded && jump)
			{
				// Add a vertical force to the player.
				animator.SetBool("IsJumping", true);
				animator.SetBool("JumpUp", true);
				m_Grounded = false;
				
				
				m_Rigidbody.AddForce(new Vector3(0f, m_JumpForce,0f));
				
				
				
				if (!Input.GetKey("space"))
				{
					m_Rigidbody.AddForce(new Vector3(0f,-(m_JumpForce/2),0));
				}
				canDoubleJump = true;
				particleJumpDown.Play();
				particleJumpUp.Play();
			}
			else if (!m_Grounded && jump && canDoubleJump && !isWallSliding)
			{
				canDoubleJump = false;
				m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, 0);
				m_Rigidbody.AddForce(new Vector2(0f, m_JumpForce / 1.2f));
				animator.SetBool("IsDoubleJumping", true);
			}

			else if (m_IsWall && !m_Grounded)
			{
				if (!oldWallSlidding && m_Rigidbody.velocity.y < 0 || isDashing)
				{
					isWallSliding = true;
					m_WallCheck.localPosition = new Vector3(-m_WallCheck.localPosition.x, m_WallCheck.localPosition.y, 0);
					Flip();
					spriteRenderer.flipX = true;
					StartCoroutine(WaitToCheck(0.1f));
					canDoubleJump = true;
					animator.SetBool("IsWallSliding", true);
				}
				isDashing = false;
				

				if (isWallSliding)
				{
					if (move * transform.localScale.x > 0.1f)
					{
						StartCoroutine(WaitToEndSliding());
						spriteRenderer.flipX = false;
					}
					else 
					{
						oldWallSlidding = true;
						m_Rigidbody.velocity = new Vector3(-transform.localScale.x * 2, -5,0);
					}
				}
				
				else if (dash && canDash)
				{
					isWallSliding = false;
					animator.SetBool("IsWallSliding", false);
					oldWallSlidding = false;
					m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
					canDoubleJump = true;
					StartCoroutine(DashCooldown());
					spriteRenderer.flipX = false;
				}
			}
			else if (isWallSliding && !m_IsWall && canCheck) 
			{
				isWallSliding = false;
				animator.SetBool("IsWallSliding", false);
				oldWallSlidding = false;
				m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
				canDoubleJump = true;
				spriteRenderer.flipX = false;
			}
			if (Input.GetButtonUp("Jump") && m_Rigidbody.velocity.y > 0)
			{
				m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_Rigidbody.velocity.y * .5f, 0);
			}
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	public void ApplyDamage(float damage, Vector3 position)
	{
		StartCoroutine(DamagePlayer(damage, position));
	}
	
	public void ApplyFallDamage(float damage) 
	{
		if (!invincible)
		{
			animator.SetBool("Hit", true);
			life -= damage;
			healthbar.loseHP(damage);

			if (life <= 0)
			{
				StartCoroutine(WaitToDead());
			}
			else
			{
				StartCoroutine(Stun(0.3f));
				StartCoroutine(MakeInvincible(0.5f));
			}
		}
	}
	
	public void ApplyGrapleForce()
	{
		StartCoroutine(GrapplePull());

	}

	public float secs = 0.01f;
	public float grappleY = 0.7f;
	IEnumerator GrapplePull()
	{
		isGrapplePulling = true;
		player.runSpeed = 0;
		
		m_Rigidbody.AddForce(new Vector3(transform.localScale.x*m_GrappleForce, grappleY, 0f), ForceMode.Impulse);
		yield return new WaitForSeconds(secs);
		
		m_Rigidbody.AddForce(new Vector3(transform.localScale.x*m_GrappleForce, grappleY, 0f), ForceMode.Impulse);
		yield return new WaitForSeconds(secs);
		
		m_Rigidbody.AddForce(new Vector3(transform.localScale.x*m_GrappleForce, grappleY, 0f), ForceMode.Impulse);
		yield return new WaitForSeconds(secs);
		
		m_Rigidbody.AddForce(new Vector3(transform.localScale.x*m_GrappleForce, grappleY, 0f), ForceMode.Impulse);
		yield return new WaitForSeconds(secs);
		
		m_Rigidbody.AddForce(new Vector3(transform.localScale.x*m_GrappleForce, grappleY, 0f), ForceMode.Impulse);
		yield return new WaitForSeconds(secs);
		
		m_Rigidbody.AddForce(new Vector3(transform.localScale.x*m_GrappleForce, grappleY, 0f), ForceMode.Impulse);
		yield return new WaitForSeconds(secs);
		
		m_Rigidbody.AddForce(new Vector3(transform.localScale.x*m_GrappleForce, grappleY, 0f), ForceMode.Impulse);
		yield return new WaitForSeconds(secs);
		
		m_Rigidbody.AddForce(new Vector3(transform.localScale.x*m_GrappleForce, grappleY, 0f), ForceMode.Impulse);
		yield return new WaitForSeconds(secs);
		
		m_Rigidbody.AddForce(new Vector3(transform.localScale.x*m_GrappleForce, grappleY, 0f), ForceMode.Impulse);
		yield return new WaitForSeconds(secs);
		
		m_Rigidbody.AddForce(new Vector3(transform.localScale.x*m_GrappleForce, grappleY, 0f), ForceMode.Impulse);
		yield return new WaitForSeconds(secs);
		
		m_Rigidbody.AddForce(new Vector3(transform.localScale.x*m_GrappleForce, grappleY, 0f), ForceMode.Impulse);
		yield return new WaitForSeconds(secs);
		
		m_Rigidbody.AddForce(new Vector3(transform.localScale.x*m_GrappleForce, grappleY, 0f), ForceMode.Impulse);
		yield return new WaitForSeconds(secs);
		
		

		
		yield return new WaitUntil(() => m_Grounded == true);
		player.runSpeed = 40;
		canDoubleJump = true;
		isGrapplePulling = false;





	}

	IEnumerator DamagePlayer(float damage, Vector3 position)
	{
		yield return new WaitForSeconds(0f);
		if (!invincible)
		{
			animator.SetBool("Hit", true);
			life -= damage;
			Vector3 knockBack = new Vector3(transform.position.x - position.x, transform.position.y - position.y, 0f);
			Vector3 damageDir = Vector3.Normalize(knockBack) * 20f ;
			m_Rigidbody.velocity = Vector3.zero;
			m_Rigidbody.AddForce(damageDir * 15);
			healthbar.loseHP(damage);
			if (life <= 0)
			{
				StartCoroutine(WaitToDead());
			}
			else
			{
				StartCoroutine(Stun(0.25f));
				StartCoroutine(MakeInvincible(0.5f));
			}
		}
	}
	
	IEnumerator HealCooldown()
	{
		
		canHeal=false;
		yield return new WaitForSeconds(0.5f);
		canHeal = true;
		healParticle.Stop();
		
	}
	IEnumerator DashCooldown()
	{
		animator.SetBool("IsDashing", true);
		isDashing = true;
		canDash = false;
		yield return new WaitForSeconds(0.2f);
		isDashing = false;
		yield return new WaitForSeconds(0.6f);
		canDash = true;
	}

	IEnumerator Stun(float time) 
	{
		canMove = false;
		yield return new WaitForSeconds(time);
		canMove = true;
	}
	IEnumerator MakeInvincible(float time) 
	{
		invincible = true;
		yield return new WaitForSeconds(time);
		invincible = false;
	}
	IEnumerator WaitToMove(float time)
	{
		canMove = false;
		yield return new WaitForSeconds(time);
		canMove = true;
	}

	IEnumerator WaitToCheck(float time)
	{
		canCheck = false;
		yield return new WaitForSeconds(time);
		canCheck = true;
	}

	IEnumerator WaitToEndSliding()
	{
		yield return new WaitForSeconds(0.1f);
		canDoubleJump = true;
		isWallSliding = false;
		animator.SetBool("IsWallSliding", false);
		oldWallSlidding = false;
		m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
	}

	IEnumerator WaitToDead()
	{
		
		animator.SetBool("IsDead", true);
		canMove = false;
		invincible = true;
		GetComponent<Attack>().enabled = false;
		yield return new WaitForSeconds(0.4f);
		m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, 0);
		yield return new WaitForSeconds(1.1f);
		life = 100f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	
	
}
