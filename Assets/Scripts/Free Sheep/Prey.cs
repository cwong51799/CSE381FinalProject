using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : MonoBehaviour
{
    public void getEaten() {
        // Play animation
        // Disappear and stop rendering
        Destroy(this.gameObject);
    }
}
