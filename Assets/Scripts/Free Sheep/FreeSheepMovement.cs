using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;



/* The sheep has a few states:
1. Grazing: When the sheep does not detect any nearby wolf. A sheep is grazing when there is no wolf detected.
            A sheep will pick a new location on the NavMesh every 10 seconds to path to.
2. Running: When the sheep has detected a wolf through hearing/sight and is broadcasting a startle.
            The difference between running and startled is subtle. A running sheep will not be able to be
            startled, meaning a sheep will focus on running away from the wolf it sees before entering a state of startle.
            A sheep is deterministically in a running state when there is a wolf detected but it is not startled.
3. Startled: When the sheep was alerted by a nearby sheep to run away. A sheep is in the startled state when there is a wolf detected an it is startled.

*/
public class FreeSheepMovement : MonoBehaviour
{
    public Camera vision;

    public GameObject wolf1, wolf2, wolf3;

    GameObject[] wolves;

    public NavMeshAgent agent;
    public float runSpeed = 10f;
    public float detectionRadius = 10f;
    public float grazingSpeed = 5f;

    public float newPathFrequency = 10f;

    public bool keepPathing = true;

    public Material grazingColor;

    public Material scaredColor;

    // Startling
    float startleTimer = 0f;

    public float startleRadius = 20f;

    bool receivingStartle = false;
    


    GameObject wolfDetected;

    // CREATE A TIER LIST IN WHAT ORDER A SHEEP WOULD RUN AWAY FROM A WOLF.
    // EX SIGHT > SOUND > STARTLE ?


    // Found here https://answers.unity.com/questions/8003/how-can-i-know-if-a-gameobject-is-seen-by-a-partic.html
    // Checks if the wolf is visible.
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


    bool isWolfAudible(GameObject wolf) {
        WolfMovementScript wolfScript = wolf.GetComponent<WolfMovementScript>();
        float dist = Vector3.Distance(this.transform.position, wolf.transform.position);
        // If the wolf is in audible distance and it's not crouching, return true.
        if(dist < wolfScript.audibleRange && !wolfScript.getCrouchingStatus()) {
            return true;
        }
        return false;
    }


    void adjustColor() {
        if (wolfDetected != null) {
            this.GetComponent<MeshRenderer>().material = scaredColor;
        } else {
            this.GetComponent<MeshRenderer>().material = grazingColor;
        }
    }

    void setAlertColor() {
        this.GetComponent<MeshRenderer>().material = scaredColor;
    }

    void setGrazeColor() {
        this.GetComponent<MeshRenderer>().material = grazingColor;
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
            if (isWolfInFrustum(wolf)) {
                return wolf;
            }
        }
        return null;
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


    // When a sheep detects a wolf, create a chain reaction startling all other sheep within a certain radius to run away from the wolf.
    // This forces the player to keep an eye out for nearby sheep alerting eachother.
    // The startle radius should somewhat scale with the sheep's FOV/view distance I think. Need to find a good number for that.
    void startleNearbySheep(GameObject wolf) {
        Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, startleRadius);
        foreach (var hitCollider in hitColliders)
        {
            // If it has a sheep movement script, it's a sheep. Otherwise the get component will return null and the rest will not execute.
            FreeSheepMovement sheepScript = hitCollider.gameObject.GetComponent<FreeSheepMovement>();
            // Startle all nearby sheep, but not yourself (would lead to infinite loop)
            if(sheepScript && hitCollider.gameObject != this.gameObject) {
                sheepScript.startle(wolfDetected);
            }
        }
    }


    // Sets the sheep's wolf detected to the original wolf for 2 seconds
    // Only startle a sheep if it wasn't currently running away from a wolf.
    public void startle(GameObject wolf) {
        if (wolfDetected == null) {
            setWolfDetected(wolf);
            startleTimer = 2;
            receivingStartle = true;
        }
    }

    // Check the startle timer, decrease it once per second. Once it times out then set receiving startle to false
    // so that the sheep can start detecting nearby wolves again.
    void checkForUnstartiling() {
        if (startleTimer > 0) {
            startleTimer -= Time.deltaTime;
        } else {
            // No longer detect the wolf
            startleTimer = 0;
            wolfDetected = null;
            receivingStartle = false;
        }
    }

    // Temporarily set their wolf detected.
    void setWolfDetected(GameObject wolf) {
        wolfDetected = wolf;
    }

    // Randomly pick a location on the NavMesh and set it as a location. Only do this if there is currently no wolf detected.
    void findANewDestination() {
        if (!wolfDetected && keepPathing) {
            // Distance the random target should be
            float walkRadius = 200;
            // Pick a random direction
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;
            // Pick a random position
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            Vector3 finalPosition = hit.position;
            // Send the sheep there.
            agent.SetDestination(finalPosition);
        }
    }

    // A sheep will no longer look for a new location if this is set to false.
    public void setKeepPathing(bool continuePathing) {
        keepPathing = continuePathing;
    }

    // Run in the opposite direction.
    public void runAway(GameObject gameObjectToRunAwayFrom) {
            float step =  -1 * runSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, gameObjectToRunAwayFrom.transform.position, step);
    }

    // Start is called before the first frame update
    void Start()
    {
        wolves = new GameObject[] {wolf1, wolf2, wolf3};
        // Occasionally find a new destination and start walking there.
        InvokeRepeating("findANewDestination", 0, newPathFrequency);
    }

    // Update is called once per frame
    void Update()
    { 
        // Run in panic if another sheep told you to.
        if (!receivingStartle) {
            wolfDetected = searchForWolves();
        }
        // Run in the opposite direction
        if (wolfDetected != null) {
            setAlertColor();
            runAway(wolfDetected);
            // Avoid extreme chain reactions of startling. Only the original detected sheep can startle others.
            if (!receivingStartle) {
                startleNearbySheep(wolfDetected);
            }
        } else {
            setGrazeColor();
        }
        checkForUnstartiling();
    }

}
