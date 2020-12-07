using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
JumpingSheepScript
    This is the script responsible for the Jumping Sheep type. When a jumping sheep
    detects a wolf, it will jump into the air for a while, discouraging the player from
    just waiting for it to land by wasting their time.
*/
public class JumpingSheepScript : MonoBehaviour
{
    public CharacterController controller;

    public GameObject soundSystem;

    public SoundContainer sounds;


    float verticalVelocity = 0;

    float gravity = 9.8f;

    float timeBeforeReturnToGraze = 0f;


    public void jump(){
        controller.enabled = true;
        if(controller.isGrounded == true) {
            verticalVelocity = 30;
            sounds.jumpingSound.Play();
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
