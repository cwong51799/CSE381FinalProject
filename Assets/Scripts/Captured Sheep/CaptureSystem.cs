using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CaptureSystem : MonoBehaviour
{
    public GameObject capturedArea;

    public GameObject capturedSheep;

    public GameObject gameSystem;

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
        addToCapturedArea(freeSheep);
        numSheepCaptured += 1;
    }


    private void Start() {
        NavMeshAgent agent = capturedSheep.GetComponent<NavMeshAgent>();
        capturedSheepAgentID = agent.agentTypeID;
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("COLLISION FOUND");
        if(other.gameObject.tag == "FreeSheep") {
            captureSheep(other.gameObject);
        }
    }
}
