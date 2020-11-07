using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FarmersRules : MonoBehaviour
{
    public GameObject UI_Phase;

    public GameObject UI_Count;

    public GameObject UI_Timer;

    public GameObject UI_Result;

    public GameObject captureObject;

    public GameObject farmer;

    CaptureSystem captureSystem;

    static float minutesToSeconds = 60;


    public int Phase1Demand = 5;

    // Timers are set in minutes and multiplied into seconds.
    public float Phase1Timer = 2;
    public int Phase2Demand = 20;

    public float Phase2Timer = 1.5f;

    public int Phase3Demand = 30;

    public float Phase3Timer = 3;
    public int Phase4Demand = 50;
    public float Phase4Timer = 5;

    public int Phase5Demand = 100;

    public float Phase5Timer = 10;

    public float timer = 0;

    public int currentPhase = 0;
    
    public float currentPhaseTimer = 0;
    
    public int currentSheepDemand = 0;

    public bool farmerReleased = false;

    public bool gameEnded = false;

    int getCurrentPhase() {
        if(timer < Phase1Timer) {
            return 1;
        } else if(timer < Phase2Timer) {
            return 2;
        } else if(timer < Phase3Timer) {
            return 3;
        } else if(timer < Phase4Timer) {
            return 4;
        } else {
            return 5;
        }
    }


    void updateUIPhase(int phase) {
        Text txt = UI_Phase.GetComponent<Text>();
        txt.text = $"Phase {phase}";
    }

    void updateUITimeLeft() {
        Text txt = UI_Timer.GetComponent<Text>();
        float timeLeft = currentPhaseTimer - timer;
        // Show time left in seconds?
        txt.text = $"Time Left: {timeLeft} seconds";
    }


    void updateSheepCount(){
        Text txt = UI_Count.GetComponent<Text>();
        txt.text = $"Sheep Demanded: {captureSystem.getNumSheepCaptured()}/{currentSheepDemand}";
    }

    void beginPhase1(){
        currentPhaseTimer = Phase1Timer;
        currentSheepDemand = Phase1Demand;
        updateUIPhase(1);
    }

    void beginPhase2(){
        currentPhaseTimer = Phase2Timer;
        currentSheepDemand = Phase2Demand;
        updateUIPhase(2);
    }

    void beginPhase3(){
        currentPhaseTimer = Phase3Timer;
        currentSheepDemand = Phase3Demand;
        updateUIPhase(3);
    }

    void beginPhase4(){
        currentPhaseTimer = Phase4Timer;
        currentSheepDemand = Phase4Demand;
        updateUIPhase(4);
    }

    void beginPhase5(){
        currentPhaseTimer = Phase5Timer;
        currentSheepDemand = Phase5Demand;
        updateUIPhase(5);
    }

    bool pleasedTheFarmer() {
        if (captureSystem.getNumSheepCaptured() >= currentSheepDemand) {
            return true;
        } else {
            return false;
        }
    }


    public void playerLosesTheGame() {
        UI_Result.GetComponent<Text>().text = "GAME OVER";
        UI_Result.SetActive(true);
        gameEnded = true;
    }

    public void playerWinsTheGame() {
        UI_Result.GetComponent<Text>().text = "THE FARMER IS GONE. YOU WIN.";
        UI_Result.SetActive(true);
        gameEnded = true;
    }


    void updateUIForFarmer() {
        UI_Phase.GetComponent<Text>().text = "THE FARMER IS ANGRY.";
        UI_Timer.GetComponent<Text>().text = "RUN FROM THE FARMER.";
        UI_Count.GetComponent<Text>().text = "CONSUME ENOUGH SHEEP TO BE ABLE TO EAT THE FARMER.";
    }

    void releaseTheFarmer() {
        farmerReleased = true;
        updateUIForFarmer();
        farmer.SetActive(true);
    }
    void checkForPhaseUpdate() {
        if(currentPhase != getCurrentPhase()) {
            currentPhase = getCurrentPhase();
            if (!pleasedTheFarmer()) {
                releaseTheFarmer();
            } else {
                switch(currentPhase) {
                    case 1:
                        beginPhase1();
                        break;
                    case 2:
                        beginPhase2();
                        break;
                    case 3:
                        beginPhase3();
                        break;
                    case 4:
                        beginPhase4();
                        break;
                    case 5:
                        beginPhase5();
                        break;
                    default:
                        break;
                }
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        captureSystem = captureObject.GetComponent<CaptureSystem>();
        // Convert all the timers to seconds.
        Phase1Timer = Phase1Timer * minutesToSeconds;
        Phase2Timer = Phase2Timer * minutesToSeconds;
        Phase3Timer = Phase3Timer * minutesToSeconds;
        Phase4Timer = Phase4Timer * minutesToSeconds;
        Phase5Timer = Phase5Timer * minutesToSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if (!farmerReleased) {
            timer += Time.deltaTime;
            checkForPhaseUpdate();
            // If the farmer still isn't released.
            if(!farmerReleased) {
                updateUITimeLeft();
                updateSheepCount();
            }
        }
    }
}
