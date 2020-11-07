using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


// Script for the camera to swap between the wolves, given that the wolf is still alive.
public class WolfSwapper : MonoBehaviour
{
    public GameObject wolf1, wolf2, wolf3;

    GameObject[] wolves;

    WolfMovementScript wolf1movement, wolf2movement, wolf3movement;

    WolfMovementScript[] wolfScripts;

    CinemachineFreeLook freeLook;
    

    // Loop through all the scripts, setting the currentWolf control to true and the rest to false;
    public void setControlTo(GameObject wolf) {
        if(wolf != null) {
            foreach(var wolfScript in wolfScripts) {
                if(wolfScript.gameObject == wolf) {
                    freeLook.LookAt = wolfScript.gameObject.transform;
                    freeLook.Follow = wolfScript.gameObject.transform;
                    wolfScript.setUnderControl(true);
                } else {
                    wolfScript.setUnderControl(false);
                }
            }
        }
    }

    void Start() {
        freeLook = this.GetComponent<CinemachineFreeLook>();
        // Capture their scripts
        wolf1movement = wolf1.GetComponent<WolfMovementScript>();
        wolf2movement = wolf2.GetComponent<WolfMovementScript>();
        wolf3movement = wolf3.GetComponent<WolfMovementScript>();

        wolves = new GameObject[]{wolf1,wolf2,wolf3};

        wolfScripts = new WolfMovementScript[] {wolf1movement, wolf2movement, wolf3movement};

        // Set current wolf as current playable character 
        setControlTo(wolf1);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            setControlTo(wolf1);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            setControlTo(wolf2);
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            setControlTo(wolf3);
        }
    }
}
