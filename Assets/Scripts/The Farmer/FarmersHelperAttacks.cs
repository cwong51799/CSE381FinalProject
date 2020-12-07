using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Freezes you in place so the farmer can come get you.
public class FarmersHelperAttacks : MonoBehaviour
{

    public FarmerMovement movementScript;

    public GameObject farmer;

    public SoundContainer sounds;
    GameObject target;

    GameObject detainedTarget;

    public float detainRadius = 3;




    void beginDetain(GameObject wolf) {
        WolfMovementScript wolfMovement = wolf.GetComponent<WolfMovementScript>();
        // DETAIN THEM.
        wolfMovement.isDetained = true;
        detainedTarget = wolf;
    }

    void detain() {
        Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, detainRadius);
        // Search for nearby prey
        foreach (var hitCollider in hitColliders){
            // THE FARMERS HELPERS WILL DETAIN THE WOLF AND NOTIFY THE FARMER TO COME.
            if(hitCollider.gameObject.tag == "Wolf") {
                  beginDetain(hitCollider.gameObject);
                  movementScript.findNextTarget();
            }
        }
    }

    void notifyTheFarmer() {
        // If the current target is NOT detained, let them know about this detained target.
        if(!farmer.GetComponent<FarmerMovement>().target.GetComponent<WolfMovementScript>().isDetained ){
            farmer.GetComponent<FarmerMovement>().target = detainedTarget;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (detainedTarget != null) {
            notifyTheFarmer();     
        }
        target = movementScript.target;
        detain();
    }
}
