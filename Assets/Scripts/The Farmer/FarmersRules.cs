using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Updates UI and keeps track of the games core rule of herding enough sheep to keep the farmer happy.
public class FarmersRules : MonoBehaviour
{
    public GameObject UI_Phase;

    public GameObject UI_Count;

    public GameObject UI_Timer;

    public GameObject UI_Result;

    public GameObject UI_LevelInfo;

    public GameObject UI_SheepConsumedInfo;

    public TextMeshPro UI_CaptureCount;

    public GameObject captureObject;

    public GameObject farmer;

    public GameObject farmerHelper1;

    public GameObject wolf1, wolf2, wolf3;


    CaptureSystem captureSystem;


    public SoundContainer sounds;

    static float minutesToSeconds = 60;


    public float phaseLengthsInMinutes = 1;

    public int initialSheepDemand = 5;

    float timer = 0f;

    double currentPhase;
    
    public float sheepDemandScale = 2;


    // A timer scale of 2 means that the player has the same amount of time as last phase to complete more sheep
    public float levelTimerScale = 1; 

    float currentPhaseTimer;
    
    int currentSheepDemand;

    bool farmerReleased = false;

    bool gameEnded = false;

    public bool getGameEnded() {
        return gameEnded;
    }

    public void setGameEnded(bool tf){
        gameEnded = tf;
    }


    double calculateCurrentPhase() {
        return Mathf.Floor(timer/phaseLengthsInMinutes) + 1;
    }


    double getCurrentPhase() {
        return calculateCurrentPhase();
    }


    void updateUIPhase(double phase) {
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
        txt.text = $"Sheep Herded: {captureSystem.getNumSheepCaptured()}/{currentSheepDemand}";
        UI_CaptureCount.text = $"{captureSystem.getNumSheepCaptured()}/{currentSheepDemand}";
    }

    void beginNextPhase(){
        sounds.newPhaseSound.Play();
        currentPhaseTimer = currentPhaseTimer + (currentPhaseTimer * levelTimerScale);
        currentSheepDemand = currentSheepDemand + Mathf.FloorToInt(currentSheepDemand * sheepDemandScale);
        updateUIPhase(currentPhase);
    }


    bool pleasedTheFarmer() {
        if (captureSystem.getNumSheepCaptured() >= currentSheepDemand) {
            return true;
        } else {
            return false;
        }
    }

    void updateUILevelInfo() {
        Text txt = UI_LevelInfo.GetComponent<Text>();
        // Use Wolf 1 as reference for their progression, all of them should be the same.
        WolfProgression progressionScript = wolf1.GetComponent<WolfProgression>();
        txt.text = $"Level 1: {progressionScript.Level1Threshold} Sheep\n" +
                   $"Level 2: {progressionScript.Level2Threshold} Sheep\n" +
                   $"Level 3: {progressionScript.Level3Threshold} Sheep\n" + 
                   $"Level 4: {progressionScript.Level4Threshold} Sheep\n";
    }


    public void playerLosesTheGame() {
        Debug.Log("PLAYER LOSES THE GAME.");
        UI_Result.GetComponent<Text>().text = "GAME OVER";
        UI_Result.SetActive(true);
        gameEnded = true;
    }

    public void playerWinsTheGame() {
        Debug.Log("PLAYER WINS THE GAME.");
        UI_Result.GetComponent<Text>().text = "THE FARMER IS GONE. YOU WIN.";
        UI_Result.SetActive(true);
        gameEnded = true;
    }


    void updateUIForFarmer() {
        UI_Phase.GetComponent<Text>().text = "THE FARMER IS ANGRY.";
        UI_Timer.GetComponent<Text>().text = "RUN FROM THE FARMER.";
        UI_Count.GetComponent<Text>().text = "CONSUME ENOUGH SHEEP TO BE ABLE TO EAT THE FARMER.";
        UI_CaptureCount.text = "THE FARMER IS ANGRY.";
    }

    void releaseTheFarmer() {
        sounds.farmerSoundtrack.Play();
        farmerReleased = true;
        updateUIForFarmer();
        farmer.SetActive(true);
        releaseHelper1();
    }

    void releaseHelper1() {
        farmerHelper1.SetActive(true);
    }

    public void updateWolfConsumationCount() {
        Text txt = UI_SheepConsumedInfo.GetComponent<Text>();
        txt.text = $"Wolf 1: {wolf1.GetComponent<WolfProgression>().getSheepConsumed()} Consumed\n" +
            $"Wolf 2: {wolf2.GetComponent<WolfProgression>().getSheepConsumed()} Consumed\n" +
            $"Wolf 3: {wolf3.GetComponent<WolfProgression>().getSheepConsumed()} Consumed\n";
    }


    void checkForPhaseUpdate() {
        if(currentPhase != calculateCurrentPhase()) {
            if(pleasedTheFarmer()) {
                currentPhase = calculateCurrentPhase();
                beginNextPhase();
            } else {
                releaseTheFarmer();
            }
        }
    }

    void checkForGameEnd() {
        GameObject[] wolves = GameObject.FindGameObjectsWithTag("Wolf");
        if (wolves.Length == 0) {
            playerLosesTheGame();
            setGameEnded(true);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        captureSystem = captureObject.GetComponent<CaptureSystem>();
        // Convert all the timers to seconds.
        phaseLengthsInMinutes = phaseLengthsInMinutes * minutesToSeconds;
        currentPhase = calculateCurrentPhase();
        currentPhaseTimer = phaseLengthsInMinutes;
        currentSheepDemand = initialSheepDemand;
        updateUIPhase(1);
        updateUILevelInfo();
    }

    // Update is called once per frame
    void Update()
    {
        checkForGameEnd();
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
