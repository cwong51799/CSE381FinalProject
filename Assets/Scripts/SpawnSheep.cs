using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class SpawnSheep : MonoBehaviour
{
    public GameObject objectToCreate;
    
    public int numberOfSheep = 5;

    Vector3 pickARandomLocation() {
        // Pick a range
        float walkRadius = 300;
        // Pick a random direction
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas);
        Vector3 location = hit.position;
        return location;
    }


    void spawnASheep() {
        Vector3 location = pickARandomLocation();
        Instantiate(objectToCreate, location, Random.rotation);
    }



    // Start is called before the first frame update
    void Start()
    {   
        for (var i=0;i<numberOfSheep;i++) {
            spawnASheep();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
