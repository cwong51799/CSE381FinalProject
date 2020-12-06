using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSheepNoise : MonoBehaviour
{
    List<AudioSource> sheepNoises;

    public float playRandomSheepNoiseEveryXSeconds = 10f;

    void playRandomSheepNoise() {
        // Shouldn't be zero.
        if(sheepNoises.Count == 0) {
            return;
        }
        int roll = Random.Range(0,sheepNoises.Count);
        AudioSource noiseToPlay = sheepNoises[roll];
        noiseToPlay.Play();
    }



    // Filters only for sheep noises
    List<AudioSource> filterForSheepNoises(AudioSource[] arr) {
        List<AudioSource> result = new List<AudioSource>();
        foreach(AudioSource source in arr) {
            if (source.tag == "sheepNoise") {
                result.Add(source);
            }
        }
        return result;
    }



    // Start is called before the first frame update
    // https://answers.unity.com/questions/546915/how-to-list-all-playing-audio-clips.html
    void Start()
    {
        AudioSource[] noises = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        sheepNoises = filterForSheepNoises(noises);
        InvokeRepeating("playRandomSheepNoise", 0, playRandomSheepNoiseEveryXSeconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
