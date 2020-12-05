using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class SheepSpawner : MonoBehaviour
{
    public GameObject objectToCreate;
    
    public int initialNumberOfSheep = 30;

    public float sheepSpawnFrequency = 15;

    public int amountOfSheepPerSpawn = 20;

    public float MAX_AMOUNT_OF_FREE_SHEEP = 100;


    // Probably have a max amount of sheep


    // Count the amount of free sheep.
    bool shouldSpawnSheep(){
        GameObject[] freesheeps = GameObject.FindGameObjectsWithTag("FreeSheep");
        if(freesheeps.Length < MAX_AMOUNT_OF_FREE_SHEEP) {
            return true;
        } else {
            return false;
        }
    }

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
        Instantiate(objectToCreate, location, Random.rotation).SetActive(true);
    }

    void spawnSheep(int amount) {
         for (var i=0;i<amount;i++) {
             if(!shouldSpawnSheep()) {
                return;
             }
             spawnASheep();
         }
    }

    void periodicallySpawnSheep() {
        spawnSheep(amountOfSheepPerSpawn);
    }


    // Start is called before the first frame update
    void Start()
    {   
        spawnSheep(initialNumberOfSheep);
        InvokeRepeating("periodicallySpawnSheep", 0, sheepSpawnFrequency);
    }
}
