using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepMovementScript : MonoBehaviour
{
    public Camera vision;

    public GameObject wolf1, wolf2, wolf3;

    public float speed = 10f;

    GameObject[] wolves;

    // Found here https://answers.unity.com/questions/8003/how-can-i-know-if-a-gameobject-is-seen-by-a-partic.html
    // Checks if the wolf is visible.
    bool isWolfVisible(GameObject wolf) {
        var planes = GeometryUtility.CalculateFrustumPlanes(vision);
        var point = wolf.transform.position;
        foreach (var plane in planes) {
            if (plane.GetDistanceToPoint(point) < 0) {
                return false;
            }
        }
        return true;
    }

    bool isWolfAudible(GameObject wolf) {
        return true;
    }

    GameObject searchForWolves() {
        foreach(var wolf in wolves) {
            if (isWolfVisible(wolf)) {
                return wolf;
            }
        }
        return null;
    }

    GameObject listenForWolves() {
        foreach(var wolf in wolves) {
            if (isWolfAudible(wolf)) {
                return wolf;
            }
        }
        return null;
    }

    void runAway(GameObject gameObjectToRunAwayFrom) {
            float step =  -1 * speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, gameObjectToRunAwayFrom.transform.position, step);
    }

    // Start is called before the first frame update
    void Start()
    {
        wolves = new GameObject[] {wolf1, wolf2, wolf3};
    }

    // Update is called once per frame
    void Update()
    { 
        GameObject wolfInSight = searchForWolves();
        // Run in the opposite direction
        if (wolfInSight != null) {
            runAway(wolfInSight);
        }
    }
}
