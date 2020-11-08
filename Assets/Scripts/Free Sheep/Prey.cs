using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : MonoBehaviour
{
    public void getEaten() {
        // Play animation
        if(this.gameObject.tag == "FreeSheep") {
            this.gameObject.SetActive(false);
        }
        if(this.gameObject.tag == "Wolf") {
            WolfProgression script = this.gameObject.GetComponent<WolfProgression>();
            script.isAlive = false;
            this.gameObject.SetActive(false);
        }
        // Disappear and stop rendering. Can check if a wolf is alive based on if != null

    }
}
