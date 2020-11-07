using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Captured Sheeps just path around within the captured area's NavMesh
public class CapturedSheepMovementScript : MonoBehaviour
{

    float newPathFrequency = 10;
    public NavMeshAgent agent;

    // Finds a new destination within its NavMesh
    void findANewDestination() {
        // Distance the random target should be
        float walkRadius = 20;
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


    // Start is called before the first frame update
    void Start()
    {
        // Occasionally find a new destination and start walking there.
        InvokeRepeating("findANewDestination", 0, newPathFrequency);
    }
    // Update is called once per frame
}
