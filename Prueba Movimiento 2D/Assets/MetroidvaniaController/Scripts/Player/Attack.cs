using System.Collections;
using System.Collections.Generic;
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

	public GameObject cam;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		m_Rigidbody2D = GetComponent<Rigidbody>();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown (KeyCode.J) && canAttack ||(Input.GetMouseButtonDown(0) && canAttack))
		{
			animator.SetBool("IsAttacking", true);
			StartCoroutine(AttackCooldown());
		}

		if (Input.GetKeyDown(KeyCode.V)||Input.GetKeyDown(KeyCode.K))
		{
			
		}
	}

	IEnumerator AttackCooldown()
	{
		canAttack=false;
		yield return new WaitForSeconds(0.35f);
		canAttack = true;
	}

	public void DoDashDamage()
	{
		dmgValue = Mathf.Abs(dmgValue);
		Collider[] collidersEnemies = Physics.OverlapSphere(attackCheck.position, 0.9f);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.tag == "Enemy")
			{
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
				CameraShake.Shake(0.25f, 4f);
				StartCoroutine(DmgIndicator());

			}
		}
	}
	IEnumerator DmgIndicator()
    {
		spriteRenderer.color = Color.red;
		yield return new WaitForSeconds(1f);
		spriteRenderer.color = Color.white;
    }
}
