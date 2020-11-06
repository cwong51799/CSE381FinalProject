using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : MonoBehaviour
{
    public void getEaten() {
        // Play animation

        // Disappear and stop rendering
        this.gameObject.SetActive(false);
        this.GetComponent<Renderer>().enabled = false;
        this.GetComponent<SheepMovementScript>().setKeepPathing(false);
    }

}
