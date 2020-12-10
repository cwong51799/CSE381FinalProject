using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Prey
    This script is responsbile for a GameObject getting "eaten." Different
    types have different behaviors, and I probably should have split this up
    into multiple scripts. But it ain't bad.
*/
public class Prey : MonoBehaviour
{
    public SoundContainer sounds;

    public void getEaten() {    
        // Play animation
        if(this.gameObject.tag == "FreeSheep") {
            this.gameObject.SetActive(false);
            this.gameObject.GetComponent<FreeSheepMovement>().keepPathing = false;
            this.gameObject.GetComponent<FreeSheepMovement>().agent.enabled = false;
            Destroy(this.gameObject);
        }
        if(this.gameObject.tag == "Wolf") {
            WolfStatus script = this.gameObject.GetComponent<WolfStatus>();
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
