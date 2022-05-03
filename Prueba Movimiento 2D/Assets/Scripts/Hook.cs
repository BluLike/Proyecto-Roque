using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private float hookForce;
    Grapple grapple;
    Rigidbody m_rigidbody;
    LineRenderer lineRenderer;
    
    
    // Start is called before the first frame update
    public void Initialize(Grapple grapple, Transform shootTransform)
    {
        transform.forward = shootTransform.forward;
        this.grapple = grapple;
        m_rigidbody = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        m_rigidbody.AddForce(transform.forward*hookForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] positions = new Vector3[]
        {
            transform.position,
            grapple.transform.position
        };
        lineRenderer.SetPositions(positions);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((LayerMask.GetMask("Anilla") & 1 << other.gameObject.layer) > 0 )
        { 
            m_rigidbody.useGravity = false;
            m_rigidbody.isKinematic = true;
            grapple.StartPull();
        }
       
    }
    
}
