using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SwapWolves : MonoBehaviour
{
    public GameObject wolf1, wolf2, wolf3;

    public WolfMovementScript wolf1movement, wolf2movement, wolf3movement;

    public GameObject currentWolf;

    
    CinemachineFreeLook freeLook;
    

    void cyclePlayerControl() {
        if(currentWolf == wolf1) {
            // Adjust camera
            freeLook.LookAt = wolf2.transform;
            freeLook.Follow = wolf2.transform;
            currentWolf = wolf2;

            // Adjust control
            wolf1movement.setUnderControl(false);
            wolf2movement.setUnderControl(true);
        } else if (currentWolf == wolf2) {
            // Adjust camera
            freeLook.LookAt = wolf3.transform;
            freeLook.Follow = wolf3.transform;
            currentWolf = wolf3;

            // Adjust control
            wolf2movement.setUnderControl(false);
            wolf3movement.setUnderControl(true);
        } else {
            // Adjust camera
            freeLook.LookAt = wolf1.transform;
            freeLook.Follow = wolf1.transform;
            currentWolf = wolf1;

            // Adjust control
            wolf3movement.setUnderControl(false);
            wolf1movement.setUnderControl(true);
        }

    }

    void Start() {
        freeLook = this.GetComponent<CinemachineFreeLook>();
        // Capture their scripts
        wolf1movement = wolf1.GetComponent<WolfMovementScript>();
        wolf2movement = wolf2.GetComponent<WolfMovementScript>();
        wolf3movement = wolf3.GetComponent<WolfMovementScript>();

        // Set current wolf as current playable character 
        currentWolf = wolf1;
        wolf1movement.setUnderControl(true);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            cyclePlayerControl();
        }
    }
}
