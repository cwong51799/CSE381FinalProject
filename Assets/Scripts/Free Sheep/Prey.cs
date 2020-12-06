using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : MonoBehaviour
{
    public SoundContainer sounds;

    public void getEaten() {    
        // Play animation
        if(this.gameObject.tag == "FreeSheep") {
            this.gameObject.SetActive(false);
            this.gameObject.GetComponent<FreeSheepMovement>().keepPathing = false;
        }
        if(this.gameObject.tag == "Wolf") {
            WolfProgression script = this.gameObject.GetComponent<WolfProgression>();
            script.isAlive = false;
            this.gameObject.SetActive(false);
        }
        // Disappear and stop rendering. Can check if a wolf is alive based on if != null
        if(this.gameObject.tag == "Farmer") {
            FarmerMovement script = this.gameObject.GetComponent<FarmerMovement>();
            this.gameObject.SetActive(false);
        }
    }
}
