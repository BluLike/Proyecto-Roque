using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine;
using UnityEngine;




	
	
public class Attack : MonoBehaviour
{
	public float dmgValue = 20;
	public GameObject grapplingHook;
	public Transform attackCheck;
	private Rigidbody m_Rigidbody;
	public Animator animator;
	public bool canAttack = true;
	public bool canGrapple = true;
	public bool canHeal = true;
	public bool isTimeToCheck = false;
	public SpriteRenderer spriteRenderer;
	public CharacterControllerNonUnity characterController;
	public float dynFriction;
	public Collider coll;
	public GameObject grappling;
	public Grapple grapple;
	public HealthBar_smooth healthbar;
	[SerializeField] private int healValue = 30;
	
	

	public GameObject cam;

	private void Awake()
	{
		characterController=GetComponent<CharacterControllerNonUnity>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		m_Rigidbody = GetComponent<Rigidbody>();
		coll = GetComponent<Collider>();
		healthbar = GetComponent<HealthBar_smooth>();
		


	}

	// Start is called before the first frame update
	void Start()
    {

	}

    // Update is called once per frame
    void Update()
    {
		
	    coll.material.dynamicFriction = dynFriction;
	    
		if (Input.GetKeyDown (KeyCode.J) && canAttack ||(Input.GetMouseButtonDown(0) && canAttack))
		{
			animator.SetBool("IsAttacking", true);
			DoDashDamage();
			dynFriction = 2f;
			StartCoroutine(AttackCooldown());
		}

		if (Input.GetMouseButtonDown(1) && canGrapple||Input.GetKeyDown(KeyCode.K) && canGrapple)
		{
			characterController.isGrapplePulling = true;
			grappling = Instantiate(grapplingHook, transform.position + new Vector3(transform.localScale.x * 0.5f,0,0), Quaternion.identity) as GameObject;
			Vector3 direction = new Vector3(transform.localScale.x, 0, 0);
            grappling.GetComponent<Grapple>().direction = direction; 
            grappling.name = "grapplingHook";
            grapple = GameObject.Find("grapplingHook").GetComponent<Grapple>();
            grapple.DestroyGrapple();
			StartCoroutine(GrappleCooldown());
			


		}

		
	}

	IEnumerator AttackCooldown()
	{
		canAttack=false;
		characterController.canMove = false;
		yield return new WaitForSeconds(0.35f);
		canAttack = true;
		characterController.canMove = true;
		dynFriction = 0.6f;
	}
	IEnumerator GrappleCooldown()
	{
		canGrapple=false;
		characterController.canMove = false;
		yield return new WaitUntil( () => grappling == null);
		canGrapple = true;
		m_Rigidbody.constraints = ~RigidbodyConstraints.FreezePositionX & ~RigidbodyConstraints.FreezePositionY & ~RigidbodyConstraints.FreezePositionZ; ;
		characterController.isGrapplePulling = false;
		characterController.canMove = true;
		



	}
	
	
	public void DoDashDamage()
	{
		dmgValue = Mathf.Abs(dmgValue);
		Collider[] collidersEnemies = Physics.OverlapSphere(attackCheck.position, 0.7f);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.tag == "Enemy")
			{
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
				CinemachineShake.Instance.ShakeCamera(1.5f,0.2f);

			}
			if (collidersEnemies[i].gameObject.layer == 12)
			{
				
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
				CinemachineShake.Instance.ShakeCamera(0.8f,0.2f);

			}
		}
	}
	
	
}
