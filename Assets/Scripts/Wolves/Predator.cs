using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour
{
    WolfProgression progressionScript;

    public FarmersRules gameSystem;

    public float eatRadius = 3f;
    // Start is called before the first frame update

    void Start()
    {
        progressionScript = this.GetComponent<WolfProgression>();
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
                    Debug.Log(preyScript.gameObject.tag);
                    // Can only eat farmer if progression level is 4
                    if(preyScript.gameObject.tag == "Farmer" && progressionScript.getWolfLevel() == 4) {
                        // Win con
                        preyScript.getEaten();
                        gameSystem.playerWinsTheGame();
                        gameSystem.gameEnded = true;
                    }
                    if(hitCollider.gameObject.tag == "FreeSheep") {
                        preyScript.getEaten();
                        progressionScript.consumeASheep();
                    }
                }
            }
        }
    }
}