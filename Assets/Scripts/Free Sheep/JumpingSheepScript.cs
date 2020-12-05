using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingSheepScript : MonoBehaviour
{
    public CharacterController controller;

    float verticalVelocity = 0;

    float gravity = 9.8f;

    float timeBeforeReturnToGraze = 0f;


    public void jump(){
        controller.enabled = true;
        if(controller.isGrounded == true) {
            verticalVelocity = 30;
        }
    }

    void fall() {
        verticalVelocity -= gravity * Time.deltaTime;
        Vector3 moveVector = new Vector3(0, verticalVelocity, 0);
        controller.Move(moveVector * Time.deltaTime);
        if(controller.isGrounded){
            controller.enabled = false;
        }
    }

    void checkForReturnToGraze(){
        // Timer for returning to grazing after jumping
        if(timeBeforeReturnToGraze > 0) {
            timeBeforeReturnToGraze -= Time.deltaTime;
        } else {
            timeBeforeReturnToGraze = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.enabled) {
            fall();
        }
    }
}
