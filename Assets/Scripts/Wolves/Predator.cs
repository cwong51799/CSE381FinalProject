using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Predator : MonoBehaviour
{
    public WolfProgressionMaster progressionScript;

    public FarmersRules gameSystem;

    public SoundContainer sounds;   

    public float eatRadius = 3f;
    // Start is called before the first frame update

    void Start()
    {
        
    }
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
                        preyScript.getEaten();
                        gameSystem.playerWinsTheGame();
                        gameSystem.setGameEnded(true);
                    }
                    if(hitCollider.gameObject.tag == "FreeSheep") {
                        // Play sheep consume sound;
                        Debug.Log("FREE SHEEP EATEN");
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