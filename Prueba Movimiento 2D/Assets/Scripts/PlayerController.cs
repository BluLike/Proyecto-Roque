using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    public float horizontal = 40f;
    public float runspeed = 10f;
    public float gravity = 2f;
    public float jumpforce = 2f;

    Vector3 PlayerInput;

    public CharacterController controller;

    public GameObject BaseTorre;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        
    }
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        PlayerInput = new Vector3(horizontal, -gravity, 0f);

        if (Input.GetButton("Jump"))
        {
            Jump();
        }

        controller.Move(PlayerInput * runspeed * Time.deltaTime);

        
    }
    void Jump()
    {
        PlayerInput.y = jumpforce;
    }

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "TriggerGiro")
        {
            Debug.Log("Colision");

            //Revisar
            BaseTorre.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, transform.rotation.y + 90f, 0f), Time.deltaTime*0.01f);
            
        }
    }
}
    
