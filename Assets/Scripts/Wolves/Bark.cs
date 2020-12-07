using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Bark
    This script manages the "F" action behavior. Barking causes all sheep within
    the given radius to become aware of the wolf and run away. Highly useful for
    leading the sheep towards the wrangling zone.
*/
public class Bark : MonoBehaviour
{

    public SoundContainer sounds;

    public float barkSoundRadius = 20f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F)) { 
            Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, barkSoundRadius);
            // Play bark sound
            if (!sounds.barkSound.isPlaying){
                sounds.barkSound.Play();
            }
            // Search for nearby sheep
            foreach (var hitCollider in hitColliders){
                // If it's edible, trigger the getEaten()
                FreeSheepMovement sheepScript = hitCollider.gameObject.GetComponent<FreeSheepMovement>();
                // Startle all nearby sheep, but not yourself (would lead to infinite loop)
                if(sheepScript) {
                    sheepScript.runAway(this.gameObject);
                    sheepScript.startle(this.gameObject);
                }
            }
        }
    }
}
