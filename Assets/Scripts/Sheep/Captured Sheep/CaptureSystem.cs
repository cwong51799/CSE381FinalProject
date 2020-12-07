using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
CaptureSystem
    This is the system in charge of bringing FreeSheep into the captured area.
    The main driver of this is a GameObject with a collision detection. A sheep
    can only be captured if a wolf is nearby to prevent sheep from being
    insta-captured if they spawn by the zone.
*/
public class CaptureSystem : MonoBehaviour
{
    public GameObject capturedArea;

    public GameObject capturedSheep;

    public GameObject gameSystem;

    public SoundContainer sounds;


    int capturedSheepAgentID;

    int numSheepCaptured = 0;

    public int getNumSheepCaptured() {
        return numSheepCaptured;
    }

    // Delete the Sheep and Instantiate a CapturedSheep within the constraint of the captured area
    void addToCapturedArea(GameObject freeSheep) {
        Destroy(freeSheep);
        Instantiate(capturedSheep, capturedArea.transform.position, Random.rotation).SetActive(true);
    }

    void captureSheep(GameObject freeSheep) {
        sounds.captureSound.Play();
        addToCapturedArea(freeSheep);
        numSheepCaptured += 1;
    }


    float nearbyRadius = 30f;

    private bool isWolvesNearby() {
        // Search for nearby wolves, only return true if one is "nearby"
        Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, nearbyRadius);
        foreach (var hitCollider in hitColliders){
            if(hitCollider.gameObject.tag == "Wolf") {
                return true;
            }
        }
        return false;
    }

    private void Start() {
        NavMeshAgent agent = capturedSheep.GetComponent<NavMeshAgent>();
        capturedSheepAgentID = agent.agentTypeID;
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "FreeSheep") {
            if (isWolvesNearby()) {
                captureSheep(other.gameObject);
            }
        }
    }
}
