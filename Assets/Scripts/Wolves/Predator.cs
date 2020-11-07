using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour
{

    WolfProgression progressionScript;
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
                Prey sheepScript = hitCollider.gameObject.GetComponent<Prey>();
                // Startle all nearby sheep, but not yourself (would lead to infinite loop)
                if(sheepScript) {
                    sheepScript.getEaten();
                    progressionScript.consumeASheep();
                }
            }
        }
    }
}