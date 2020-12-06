using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Provides all the sounds through public access.
// Just provide the sound user the sound container and have it access it from here.
public class SoundContainer : MonoBehaviour
{
    public AudioSource jumpingSound;
    
    public AudioSource levelUpSound;

    public AudioSource captureSound;

    public AudioSource wolfDeathSound;

    public AudioSource sheepConsumeSound;

    public AudioSource newPhaseSound;

}
