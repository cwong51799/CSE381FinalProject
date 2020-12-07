using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
FarmersAttack
    This script is responsible for having the farmer constantly "swing" at nearby targets.
    The farmer, in rage, also kills nearby sheep. This is to make it even more difficult for
    the player to consume sheep once the farmer comes out.
    If the farmer slashes a wolf, and the wolf is not yet level 4, then the wolf is killed and
    the farmer will immediately look for the next target.
*/


public class FarmersAttack : MonoBehaviour
{
    public GameObject wolf1, wolf2, wolf3;
    GameObject[] wolves;

    GameObject target;

    // For updating the camera when a wolf dies.
    public WolfSwapper swapper;

    public float attackRadius;

    public FarmersRules gameSystem;

    public FarmerMovement movementScript;

    public SoundContainer sounds;

    public WolfProgressionMaster progressionScript;

    void makeTheWolvesPrey() {
        foreach(GameObject wolf in wolves) {
            // If a wolf is level 4, it CANNOT be killed.
            if (progressionScript.getWolfLevel() < 4) {
                wolf.AddComponent<Prey>();
            }
        }        
    }


    void swing() {
        Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, attackRadius);
        // Search for nearby prey
        foreach (var hitCollider in hitColliders){
            // THE FARMER WILL RAVAGE ALL THINGS. EVEN SHEEP (which makes it even harder for you to grow while being chased!)
            Prey script = hitCollider.gameObject.GetComponent<Prey>();
            if(script && script.gameObject != this.gameObject) {
                // Get eaten if your a prey, unless your a farmer. Farmer's don't eat farmers.
                if(hitCollider.gameObject.tag != "Farmer") {
                    script.getEaten();
                }
                if(hitCollider.gameObject.tag == "Wolf") {
                    // If the tag is wolf, a wolf just died. Find the next target.
                    sounds.wolfDeathSound.Play();
                    target = movementScript.findNextTarget();
                    if (target) {
                        swapper.setControlTo(target);
                    }
                }
            }
        }
    }

    void syncTarget() {
        target = movementScript.target;
    }

    // Start is called before the first frame update
    void Start()
    {
        wolves = new GameObject[] {wolf1, wolf2, wolf3};
        makeTheWolvesPrey();
    }

    // Update is called once per frame
    void Update()
    {
        syncTarget();
        swing();
    }
}
