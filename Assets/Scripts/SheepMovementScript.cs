using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepMovementScript : MonoBehaviour
{
    public Camera vision;

    public GameObject wolf1, wolf2, wolf3;

    public NavMeshAgent agent;

    public float audibleDistance = 10f;
    public float runSpeed = 10f;

    public float grazingSpeed = 5f;

    public float turnFrequency = 3f;
    float time = 0.0f;




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
        float dist = Vector3.Distance(this.transform.position, wolf.transform.position);
        if(dist < audibleDistance) {
            return true;
        }
        return false;
    }


    GameObject searchForWolves() {
        // Look first, then listen if nothing is found.
        GameObject wolfFound = lookForWolves();
        if (wolfFound == null) {
            wolfFound = listenForWolves();
        }
        return wolfFound;
    }

    GameObject lookForWolves() {
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
                if(wolf.GetComponent<WolfMovementScript>().isAudible()) {
                    return wolf;
                }
            }
        }
        return null;
    }


    void moveForward() {
        this.transform.position += Vector3.forward * Time.deltaTime * grazingSpeed;
    }

    void moveBackward() {
        this.transform.position += -Vector3.forward * Time.deltaTime * grazingSpeed;
    }

    void moveLeft() {
        this.transform.position += Vector3.left * Time.deltaTime * grazingSpeed;
    }

    void moveRight() {
        this.transform.position += Vector3.right * Time.deltaTime * grazingSpeed;
    }
    
    void graze() {
        //moveForward();
        moveBackward();
        //moveLeft();
        moveRight();

    }

    void turnOccasionally() {
        transform.Rotate(0,90,0);
    }

    void findADestination() {

    }


    void runAway(GameObject gameObjectToRunAwayFrom) {
            float step =  -1 * runSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, gameObjectToRunAwayFrom.transform.position, step);
    }

    // Start is called before the first frame update
    void Start()
    {
        wolves = new GameObject[] {wolf1, wolf2, wolf3};
        InvokeRepeating("turnOccasionally",0,2);
        InvokeRepeating("findADestination",0,30);
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
