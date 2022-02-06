using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

/*
SheepSpawner
    This script is responsible for spawning in clones of the provided sheeps.
*/
public class SheepSpawner : MonoBehaviour
{
    public GameObject plainSheepToCreate;
    
    public GameObject jumpingSheepToCreate;

    public int initialNumberOfSheep = 500;

    public int initialNumberOfJumpingSheep = 5;

    public float sheepSpawnFrequency = 15;

    public int amountOfPlainSheepPerSpawn = 20;

    public int amountOfJumpingSheepPerSpawn = 5;

    public float MAX_AMOUNT_OF_FREE_SHEEP = 10000;

    private GameObject sheepParent;


    // Probably have a max amount of sheep


    // Count the amount of free sheep.
    bool shouldSpawnSheep(){
        Debug.Log(sheepParent.transform.childCount);
        if(sheepParent.transform.childCount < MAX_AMOUNT_OF_FREE_SHEEP) {
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


    void spawnASheep(GameObject sheepToSpawn) {
        Vector3 location = pickARandomLocation();
        GameObject newSheep = Instantiate(sheepToSpawn, location, Random.rotation);
        newSheep.GetComponent<TargettingBeamContainer>().targettingBeam.SetActive(false);
        newSheep.transform.parent = sheepParent.transform;
        newSheep.SetActive(true);
    }



    void spawnPlainSheep(int amount) {
         for (var i=0;i<amount;i++) {
             spawnASheep(plainSheepToCreate);
         }
    }

    void spawnJumpingSheep(int amount) {
        for (var i=0;i<amount;i++) {
             spawnASheep(jumpingSheepToCreate);
         }
    }


    void periodicallySpawnSheep() {
        if (shouldSpawnSheep()) {
            spawnJumpingSheep(amountOfJumpingSheepPerSpawn);
            spawnPlainSheep(amountOfPlainSheepPerSpawn);
        }
    }


    // Start is called before the first frame update
    void Start()
    {   
        sheepParent = GameObject.Find("SheepParent");
        spawnPlainSheep(initialNumberOfSheep);
        spawnJumpingSheep(initialNumberOfJumpingSheep);
        InvokeRepeating("periodicallySpawnSheep", 20, sheepSpawnFrequency);
    }
}
