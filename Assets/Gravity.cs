using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float fallRate = -9.8f;
    // Update is called once per frame
    void Update()
    {   
        Debug.Log(controller.transform.position.y);
        if(controller.transform.position.y > 0) {
            var moveVec = new Vector3(0,fallRate,0);
            controller.Move(moveVec*Time.deltaTime);
        }
    }
}
