using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfProgression : MonoBehaviour
{

    WolfMovementScript movementScript;

    Predator predatorScript;

    public int Level1Threshold = 0;

    public int Level2Threshold = 1;

    public int Level3Threshold = 2;

    // At level 4, the wolf can eat the farmhand. Remove the Prey script if there is one.
    public int Level4Threshold = 3;

    int sheepConsumed = 0;

    int wolfLevel = 1;

    public float levelScaleFactor = 1.25f;

    public bool isAlive = true;

    // Every level, gain increased stamina, speed, and stamina regen.
    void reachLevel2() {
        levelUpStats();
        grow();
        wolfLevel = 2;
    }

    void reachLevel3(){
        levelUpStats();
        grow();
        wolfLevel = 3;
    }

    void reachLevel4() {
        levelUpStats();
        grow();
        wolfLevel = 4;
        shakeOffAnyPreyComponents();
    }


    void shakeOffAnyPreyComponents() {
        Prey component = this.gameObject.GetComponent<Prey>();
        if(component) {
            Destroy(this.gameObject.GetComponent<Prey>());
        }
    }

    void grow() {
        // Make the wolf larger and louder.
        this.gameObject.transform.localScale += new Vector3(.25f,.25f,.25f);
        this.gameObject.transform.position += new Vector3(0,-.25f,0);
        movementScript.audibleRange = movementScript.audibleRange * levelScaleFactor;
    }

    // Level up stats
    void levelUpStats() {
        movementScript.maxStamina = movementScript.maxStamina * levelScaleFactor;
        movementScript.baseSpeed = movementScript.baseSpeed * levelScaleFactor;
        movementScript.staminaRegenRate = movementScript.staminaRegenRate * levelScaleFactor;
    }


    public void checkForLevelUp() {
        if (sheepConsumed >= Level4Threshold && wolfLevel == 3){
            reachLevel4();
        }
        else if(sheepConsumed >= Level3Threshold && wolfLevel == 2){
            reachLevel3();
        } 
        else if(sheepConsumed >= Level2Threshold && wolfLevel == 1) {
            reachLevel2();
        }
        
    }

    public int getWolfLevel() {
        return wolfLevel;
    }

    public void consumeASheep(){ 
        sheepConsumed += 1;
        checkForLevelUp();
    }

    private void Start() {
        // Grab a reference to the movement of this wolf. Will be used in upgrading.
        movementScript = this.GetComponent<WolfMovementScript>();
        predatorScript = this.GetComponent<Predator>();
    }

}
