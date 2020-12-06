using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
