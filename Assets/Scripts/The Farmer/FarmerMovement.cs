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

    // For updating the camera when a wolf dies.
    public WolfSwapper swapper;

    public float attackRadius;

    public FarmersRules gameSystem;

    void makeTheWolvesPrey() {
        foreach(GameObject wolf in wolves) {
            // If a wolf is level 4, it CANNOT be killed.
            if (wolf.GetComponent<WolfProgression>().getWolfLevel() < 4) {
                wolf.AddComponent<Prey>();
            }
        }        
    }


    void swing() {
        Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, attackRadius);
        // Search for nearby prey
        foreach (var hitCollider in hitColliders){
            // THE FARMER WILL RAVAGE ALL THINGS. EVEN SHEEP (which makes it even harder for you to grow while being chased!)
            Prey script = hitCollider.gameObject.GetComponent<Prey>();
            if(script && script.gameObject != this.gameObject) {
                // Get eaten if your a prey.
                script.getEaten();
                if(hitCollider.gameObject.tag == "Wolf") {
                    // If the tag is wolf, a wolf just died. Find the next target.
                    target = findNextTarget();
                    if (target) {
                        swapper.setControlTo(target);
                    } else {
                        // Player loses the game
                        gameSystem.playerLosesTheGame();
                        gameSystem.setGameEnded(true);
                    }
                }
            }
        }
    }


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
            if (wolf != null) {
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
                if (isWolfAudible(wolf)) {
                    if(wolf.GetComponent<WolfMovementScript>().isAudible()) {
                        return wolf;
                    }
                }
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


    // If there are no more targets available, the player loses. (If this returns null)
    GameObject findNextTarget() {
        foreach(GameObject wolf in wolves) {
            // If the wolf is alive
            if (wolf.gameObject.GetComponent<WolfProgression>().isAlive) {
                return wolf;
            }
        }
        return null;
    }

    void huntTheWolves(){
        if(target != null) {
            agent.SetDestination(target.transform.position);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        target = wolf1;
        wolves = new GameObject[] {wolf1, wolf2, wolf3};
        makeTheWolvesPrey();
        if(!gameSystem.getGameEnded()) {
            InvokeRepeating("huntTheWolves", 0, updateFrequency);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject wolfSensed = searchForWolves();
        if(wolfSensed != null) {
            target = wolfSensed;
            huntTheWolves();
        }
        swing();
    }
}
