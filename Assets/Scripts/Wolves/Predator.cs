using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Predator
    This script handles the "E" action behavior. A wolf will attempt to eat all
    sheep within the given eat radius. It will then notify the necessary systems for updates.
    If the wolf is level 4, then it will be able to consume both the farmer and the farmer's helper.
*/
public class Predator : MonoBehaviour
{
    public WolfProgressionMaster progressionScript;

    public FarmersRules gameSystem;

    public SoundContainer sounds;   

    public float eatRadius = 3f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        // Press E to eat.
        if (Input.GetKey(KeyCode.E)) { 
            Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, eatRadius);
            // Search for nearby sheep
            foreach (var hitCollider in hitColliders){
                // If it's edible, trigger the getEaten()
                Prey preyScript = hitCollider.gameObject.GetComponent<Prey>();
                // Consume all Prey that isn't a wolf.
                if(preyScript && hitCollider.gameObject.tag != "Wolf") {
                    // Can only eat farmer if progression level is 4
                    if(preyScript.gameObject.tag == "Farmer" && progressionScript.getWolfLevel() == 4) {
                        // Win con
                        sounds.sheepConsumeSound.Play();
                        preyScript.getEaten();
                    }
                    if(hitCollider.gameObject.tag == "FreeSheep") {
                        // Play sheep consume sound;
                        sounds.sheepConsumeSound.Play();
                        preyScript.getEaten();
                        progressionScript.consumeASheep();
                        gameSystem.updateWolfConsumationCount();
                    }
                }
            }
        }
    }
}