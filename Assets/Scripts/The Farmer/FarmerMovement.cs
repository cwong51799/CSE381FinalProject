using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/* 
FarmerMovement
    This is responsible for the Farmer and the Farmer's Helper's movement.
    Farmers will constantly hunt down living wolves by updating their destination to the targets position.
    If a Farmer senses while targetting another wolf (in FOV) then it will swap targets to the sensed wolf.
    If the wolf the farmer is targetting is killed, it will find the next available alive wolf to target.
*/
public class FarmerMovement : MonoBehaviour
{
    public Camera vision;

    public GameObject wolf1, wolf2, wolf3;
    GameObject[] wolves;

    public NavMeshAgent agent;

    public GameObject target;

    public FarmersRules gameSystem;

    GameObject searchForWolves() {
        // Look first, then listen if nothing is found.
        GameObject wolfFound = lookForWolves();
        if (wolfFound == null) {
            wolfFound = listenForWolves();
        }
        return wolfFound;
    }

    GameObject lookForWolves() {
        foreach(var wolf in wolves) {
            // Check if it's been destroyed
            if (wolf != null && !wolf.gameObject.GetComponent<WolfMovement>().isDetained) {
                if (isWolfInFrustum(wolf)) {
                    return wolf;
                }
            }
        }
        return null;
    }

    
    GameObject listenForWolves() {
        foreach(var wolf in wolves) {
            if (wolf != null) {
                if (isWolfAudible(wolf) && !wolf.gameObject.GetComponent<WolfMovement>().isDetained) {
                    if(wolf.GetComponent<WolfMovement>().isAudible()) {
                        return wolf;
                    }
                }
            }
        }
        return null;
    }

    bool isWolfAudible(GameObject wolf) {
        WolfMovement wolfScript = wolf.GetComponent<WolfMovement>();
        float dist = Vector3.Distance(this.transform.position, wolf.transform.position);
        // If the wolf is in audible distance and it's not crouching, return true.
        if(dist < wolfScript.audibleRange && !wolfScript.getCrouchingStatus()) {
            return true;
        }
        return false;
    }


    bool isWolfInFrustum(GameObject wolf) {
        var planes = GeometryUtility.CalculateFrustumPlanes(vision);
        var point = wolf.transform.position;
        foreach (var plane in planes) {
            if (plane.GetDistanceToPoint(point) < 0) {
                return false;
            }
        }
        return isWolfVisible(wolf);
    }

    bool isWolfVisible(GameObject wolf) {
        // Draw a ray from the sheep towards the wolf's direction if it's in the frustum. If it is not the first 
        // location that the ray hits, then the wolf is occluded (behind something else).
        RaycastHit hit;
        var rayDirection = wolf.transform.position - this.gameObject.transform.position;
        if (Physics.Raycast (this.gameObject.transform.position, rayDirection, out hit)) {
            if (hit.transform == wolf.transform) {
                return true;
            } else {
                //Debug.Log("WOLF IS OCCLUDED");
            }
        }
        return false;
    }


    // If there are no more targets available, the player loses. (If this returns null)
    public GameObject findNextTarget() {
        foreach(GameObject wolf in wolves) {
            // If the wolf is alive
            if (wolf.gameObject.GetComponent<WolfStatus>().isAlive && !wolf.gameObject.GetComponent<WolfMovement>().isDetained) {
                target = wolf;
                return wolf;
            }
        }
        return null;
    }

    void huntTheWolves(){
        if(target != null && !gameSystem.getGameEnded()) {
            agent.SetDestination(target.transform.position);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        target = wolf1;
        wolves = new GameObject[] {wolf1, wolf2, wolf3};
    } 

    // Update is called once per frame
    void Update()
    {
        // Solves weird case where the new target isn't getting found.
        if (target != null && !target.GetComponent<WolfStatus>().isAlive) {
            findNextTarget();
        }
        GameObject wolfSensed = searchForWolves();
        if(wolfSensed != null) {
            target = wolfSensed;
        }
        huntTheWolves();
    }
}
