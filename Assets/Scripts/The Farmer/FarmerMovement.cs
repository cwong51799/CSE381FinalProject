using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// Kinda poetic how most of these methods came from the sheep and now the wolf is the prey.
public class FarmerMovement : MonoBehaviour
{
    public Camera vision;

    public GameObject wolf1, wolf2, wolf3;
    GameObject[] wolves;

    public NavMeshAgent agent;

    public float updateFrequency = 20;

    GameObject target;



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
            if (isWolfInFrustum(wolf)) {
                return wolf;
            }
        }
        return null;
    }

    bool isWolfAudible(GameObject wolf) {
        WolfMovementScript wolfScript = wolf.GetComponent<WolfMovementScript>();
        float dist = Vector3.Distance(this.transform.position, wolf.transform.position);
        // If the wolf is in audible distance and it's not crouching, return true.
        if(dist < wolfScript.audibleRange && !wolfScript.getCrouchingStatus()) {
            return true;
        }
        return false;
    }

    GameObject listenForWolves() {
        foreach(var wolf in wolves) {
            if (isWolfAudible(wolf)) {
                if(wolf.GetComponent<WolfMovementScript>().isAudible()) {
                    return wolf;
                }
            }
        }
        return null;
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
                //Debug.Log("WOLF IS VISIBLE");
                return true;
            } else {
                //Debug.Log("WOLF IS OCCLUDED");
            }
        }
        return false;
    }

    void huntTheWolves(){
        agent.SetDestination(target.transform.position);
    }

    // Start is called before the first frame update
    void Start()
    {
        target = wolf1;
        wolves = new GameObject[] {wolf1, wolf2, wolf3};
        InvokeRepeating("huntTheWolves", 0, updateFrequency);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject wolfSensed = searchForWolves();
        if(wolfSensed != null) {
            target = wolfSensed;
            huntTheWolves();
        }
    }
}
