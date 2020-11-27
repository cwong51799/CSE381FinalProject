using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingSheepScript : MonoBehaviour
{
    public CharacterController controller;


    float verticalVelocity = 0;

    float gravity = 9.8f;

    public void jump(){
        if(controller.isGrounded == true) {
            verticalVelocity = 14;
        }
    }

    void fall() {
        verticalVelocity -= gravity * Time.deltaTime;
        Vector3 moveVector = new Vector3(0, verticalVelocity, 0);
        controller.Move(moveVector * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        fall();
    }
}
