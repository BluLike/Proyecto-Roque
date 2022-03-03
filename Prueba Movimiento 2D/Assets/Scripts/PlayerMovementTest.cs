using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTest : MonoBehaviour
{
    public float horizontal = 40f;
    public float runspeed = 10f;
    public float gravity = 2f;

    Vector3 PlayerInput;

    public CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        PlayerInput = new Vector3(horizontal, -gravity, 0f);



        controller.Move(PlayerInput * runspeed * Time.deltaTime);


    }
}
    
