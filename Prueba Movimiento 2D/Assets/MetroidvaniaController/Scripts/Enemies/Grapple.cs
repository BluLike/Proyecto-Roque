using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Grapple : MonoBehaviour
{
	[SerializeField] float pullSpeed = 0.5f;
	[SerializeField] float stopDistance = 4f;
	[SerializeField] GameObject hookPrefab;
	[SerializeField] Transform shootTrransform;

	Hook hook;
	bool pulling;
	Rigidbody m_Rigidbody;
	

// Start is called before the first frame update
	void Start()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
		pulling = false;
		
	}

	// Update is called once per frame
	void Update()
	{
		if(hook== null && Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.K) || Input.GetMouseButtonDown(1) )
		{
			StopAllCoroutines();
			pulling = false;
			hook = Instantiate(hookPrefab, shootTrransform.position, quaternion.identity).GetComponent<Hook>();
			hook.Initialize(this, shootTrransform);
			StartCoroutine(DestroyHookAfterLifetime());
		}

		if (!pulling || hook == null) return;

		if (Vector3.Distance(transform.position, hook.transform.position) <= stopDistance)
		{
			DestroyHook();
		}
		else
		{
			m_Rigidbody.AddForce((hook.transform.position - transform.position).normalized * pullSpeed, ForceMode.VelocityChange);
		}
	}

	public void StartPull()
	{
		pulling = true;
	}

	private void DestroyHook()
	{
		if (hook == null) return;

		pulling = false;
		Destroy(hook.gameObject);
		hook = null;

	}

	private IEnumerator DestroyHookAfterLifetime()
	{
		yield return new WaitForSeconds(8f);
		DestroyHook();
	}

}