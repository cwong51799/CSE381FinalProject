using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CaptureArea : MonoBehaviour
{
    public GameObject capturedArea;

    public GameObject capturedSheep;

    int capturedSheepAgentID;

    int numSheepCaptured = 0;



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
        if(other.gameObject.tag == "FreeSheep") {
            captureSheep(other.gameObject);
        }
    }
}
