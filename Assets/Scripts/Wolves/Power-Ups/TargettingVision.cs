using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
TargettingVision
    This script provides the behavior for the "Q" action. The script toggles on
    all the sheep's detection beams and then manages the cooldown and duration for it.
*/
public class TargettingVision : MonoBehaviour
{
    bool visionOn;

    bool visionReady = true;

    public float visionDuration = 5f;

    public float visionCooldown = 5f;

    float visionCooldownTimer = 0f;

    float visionTimer = 0f;

    public void turnOnTargettingBeams() {
        GameObject[] freeSheep = GameObject.FindGameObjectsWithTag("FreeSheep");
        // Iterate through all the free sheep, toggling on their beams
        foreach(GameObject sheep in freeSheep) {
             GameObject targettingBeam = sheep.GetComponent<TargettingBeamContainer>().targettingBeam;
             targettingBeam.SetActive(true);
        }
        visionReady = false;
        visionOn = true;
    }
    
    public void turnOffTargettingBeams() {
        GameObject[] freeSheep = GameObject.FindGameObjectsWithTag("FreeSheep");
        // Iterate through all the free sheep, toggling on their beams
        foreach(GameObject sheep in freeSheep) {
             GameObject targettingBeam = sheep.GetComponent<TargettingBeamContainer>().targettingBeam;
             targettingBeam.SetActive(false);
        }
        visionOn = false;
    }


    void startCooldown() {
        visionCooldownTimer = 0;
    }


    void checkForCooldown() {
        if (visionReady == false && visionOn == false && visionCooldownTimer < visionCooldown) {
            visionCooldownTimer += Time.deltaTime;
        }
        if (visionReady == false && visionOn == false && visionCooldownTimer >= visionCooldown) {
            visionReady = true;
        }
    }    

    void updateTargettingVisionTimer() {
        // If vision is on, keep incrementing it until it times out.
        if (visionOn == true && visionTimer < visionDuration) {
            visionTimer += Time.deltaTime;
        }
        if(visionOn == true && visionTimer >= visionDuration) {
            // Turn off the beams and start the cooldown.
            turnOffTargettingBeams();
            visionTimer = 0;
            visionOn = false;
            startCooldown();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q)) {
            if(visionOn == false && visionReady == true){
                turnOnTargettingBeams();
            }
        }
        updateTargettingVisionTimer();
        checkForCooldown();
    }
}
