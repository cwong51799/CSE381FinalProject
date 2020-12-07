using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
FarmersHelperAttacks
    This script is responsible for the actions that a Farmer's Helper takes. The helper 
    constantly attempts to "detain" nearby wolves. If the helper successfully detains a 
    wolf, he will notify the farmer by updating the farmer's target to the detained wolf.
    The farmer will only respond to this if it's current target is not detained. This is
    to prevent a swift double detainment from bugging out the farmer.
    A Level 4 wolf cannot be detained.
*/
public class FarmersHelperAttacks : MonoBehaviour
{

    public FarmerMovement movementScript;

    public GameObject farmer;

    public SoundContainer sounds;
    GameObject target;

    GameObject detainedTarget;

    public float detainRadius = 3;


    void beginDetain(GameObject wolf) {
        WolfMovement wolfMovement = wolf.GetComponent<WolfMovement>();
        // DETAIN THEM.
        wolfMovement.isDetained = true;
        detainedTarget = wolf;
    }

    void detain() {
        Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, detainRadius);
        // Search for nearby prey
        foreach (var hitCollider in hitColliders){
            // THE FARMERS HELPERS WILL DETAIN THE WOLF AND NOTIFY THE FARMER TO COME.
            if(hitCollider.gameObject.tag == "Wolf" && hitCollider.gameObject.GetComponent<Prey>() != null) {
                  beginDetain(hitCollider.gameObject);
                  movementScript.findNextTarget();
                  Debug.Log(movementScript.target);
            }
        }
    }

    void notifyTheFarmer() {
        // If the current target is NOT detained, let them know about this detained target.
        if(!farmer.GetComponent<FarmerMovement>().target.GetComponent<WolfMovement>().isDetained){
            farmer.GetComponent<FarmerMovement>().target = detainedTarget;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (detainedTarget != null) {
            if (detainedTarget.GetComponent<WolfMovement>().isDetained) {
                notifyTheFarmer();    
            } else {
                detainedTarget = null;
            }
        }
        target = movementScript.target;
        detain();
    }
}
