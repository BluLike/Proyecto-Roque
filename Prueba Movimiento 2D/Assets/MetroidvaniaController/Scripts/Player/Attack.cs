using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;




	
	
public class Attack : MonoBehaviour
{
	public float dmgValue = 4;
	public GameObject throwableObject;
	public Transform attackCheck;
	private Rigidbody m_Rigidbody2D;
	public Animator animator;
	public bool canAttack = true;
	public bool isTimeToCheck = false;
	public SpriteRenderer spriteRenderer;
	public CharacterController characterController;
	public float dynFriction;
	public Collider coll;

	public GameObject cam;

	private void Awake()
	{
		characterController=GetComponent<CharacterController>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		m_Rigidbody2D = GetComponent<Rigidbody>();
		coll = GetComponent<Collider>();
		
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

		if (Input.GetKeyDown(KeyCode.V)||Input.GetKeyDown(KeyCode.K))
		{
			
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
		}
	}
	
	
}
