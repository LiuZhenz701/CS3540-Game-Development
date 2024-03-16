using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {

    private float playerSpeed = 5f;

    private float jumpHeight = 2f;
    private float gravity = 9.81f;
    private float airSpeed = 2.5f;

    private float doubleSpeedDuration;
    private float doubleSpeedStartingTime;
    private Boolean doubleSpeedOn = false;

    private CharacterController controller;
    private Vector3 input, direction;
    
    void Start() {
        controller = GetComponent<CharacterController>();    
    }

    void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        input = (transform.right * horizontal + transform.forward * vertical).normalized;

        if (doubleSpeedOn && (Time.time - doubleSpeedStartingTime <= doubleSpeedDuration)) {
            playerSpeed = 10;
        } else {
            ResetSpeed();
        }

        // Press shift to accelerate
        if (Input.GetKey(KeyCode.LeftShift)) {
            input *= playerSpeed * 1.5f;
        } else {
            input *= playerSpeed;
        }
        
        // Jumping, moving while in the air
        if (controller.isGrounded) {
            direction = input;
            if (Input.GetButton("Jump")) {
                direction.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            } else {
                direction.y = 0.0f;
            }
        } else {
            input.y = direction.y;
            direction = Vector3.Lerp(direction, input, airSpeed * Time.deltaTime);
        }

        // Falling back to the ground
        direction.y -= gravity * 2 * Time.deltaTime;
        controller.Move(direction * Time.deltaTime);
    }

    public void DoubleSpeed(float duration) {
        doubleSpeedDuration = duration;
        doubleSpeedOn = true;
        doubleSpeedStartingTime = Time.time;
        Debug.Log("Double speed activated. Time: " + Time.time + ", Duration: " + duration);
    }

    private void ResetSpeed() {
        playerSpeed = 5f;
        doubleSpeedOn = false;
        Debug.Log("End time" +  Time.time);
    }
}