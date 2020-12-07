using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Gravity
    This script makes a character controller fall when it's in the air.
*/
public class Gravity : MonoBehaviour
{
    public CharacterController controller;
    // Start is called before the first frame update

    public float fallRate = -9.8f;
    // Update is called once per frame
    void Update()
    {   
        if(controller.transform.position.y > 0) {
            var moveVec = new Vector3(0,fallRate,0);
            controller.Move(moveVec*Time.deltaTime);
        }
    }
}
