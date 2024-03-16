using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public Animator anim;
    public LayerMask layerMask;
    public float jumpHeight = 10f;
    public float gravity = 9.81f;
    public float moveSpeed = 10f;
    public float airControl = 5f;
    CharacterController controller;
    Vector3 input, moveDirection;
    public Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        //movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        input = transform.right * moveHorizontal + transform.forward * moveVertical;
        input.Normalize();
        input = input * moveSpeed;
        this.anim.SetFloat("vertical", moveVertical);
        this.anim.SetFloat("horizontal", moveHorizontal);


        //jumping 
        if (controller.isGrounded)
        {
            moveDirection = input;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                if (!this.anim.GetBool("jump"))  // Check if jump animation is not already playing
                {
                    this.anim.SetBool("jump", true);  // Set jump animation to true only when starting to jump
                }
            }
            else
            {
                moveDirection.y = 0.0f;
                if (this.anim.GetBool("jump"))  // Check if jump animation is currently playing
                {
                    this.anim.SetBool("jump", false);  // Set jump animation to false when not jumping
                }
            }
        }
        else
        {
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }



        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);


        
    }
}
