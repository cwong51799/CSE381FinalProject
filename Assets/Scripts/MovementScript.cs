using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    // If this is the 1 of 3 wolves currently under control
    public bool isUnderControl = false;

    public CharacterController controller;

    public Transform cam;

    float turnSmoothTime = 0.1f;

    float turnSmoothVelocity;

    public float baseSpeed = 10;

    public float movingSpeed;
    
    // Make some of these private later on.

    // Keeps track of the maximum amount of stamina
    public double maxStamina = 100;

    // Keeps track of how much stamina the player currently has.
    public double currentStamina;

    // Rates for increasing/decreasing stamina
    public float staminaRegenRate = .01f;

    public float staminaDepletionRate = .01f;

    // Show different animation for crouching and running
    public bool isRunning = false;


    public bool isCrouching = false;


    public void setUnderControl(bool controlled) {
        isUnderControl = controlled;
    }


    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        currentStamina = maxStamina;
        movingSpeed = baseSpeed;
    }
    

    void regenerateStamina() {
        currentStamina = currentStamina + staminaRegenRate;
        if (currentStamina > maxStamina) {
            currentStamina = maxStamina;
        }
    }

    void depleteStamina() {
        currentStamina = currentStamina - staminaDepletionRate;
        if (currentStamina < 0) {
            currentStamina = 0;
        }
    }

    void adjustStamina() {
        if (isRunning) {
            depleteStamina();
        } else {
            regenerateStamina();
        }
    }

    void adjustMovespeed() {
        if(isRunning) {
            movingSpeed = baseSpeed * 2;
        } else {
            movingSpeed = baseSpeed;
        }
    }

    // Try to get this separate from frame rate with Time.deltaTime somehow.
    void handleSprinting() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            // Drain through stamina to increase speed
            // Can't run while crouching!
            if (currentStamina > 0 && !isCrouching) {
                isRunning = true;
            } 
            else {
               isRunning = false;
            }
        } else {
            // Regenerate stamina
            isRunning = false;
        }
        adjustStamina();
        adjustMovespeed();
    }


    void handleCrouching() {
        if (Input.GetKey(KeyCode.C)) {
            isCrouching = true;   
        } else {
            isCrouching = false;
        }
    }

    void handleMoveDirection() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        if(direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * movingSpeed * Time.deltaTime);
        }
    }

    // Update is called once per frame
    // Followed from https://www.youtube.com/watch?v=4HpC--2iowE
    void Update() {
        if (isUnderControl) {
            handleSprinting();
            handleCrouching();
            handleMoveDirection();
        } else {
            // Regenerate stamina passively when not in control.
            isRunning = false;
            adjustStamina();
        }
    }
}
