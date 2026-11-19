
using System;
using System.Collections;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    public int TimeLeft = 600; // in seconds
    public ClockCanvas clockCanvas;
    private Coroutine timerCoroutine; // acts as our timer that goes every second

    private IEnumerator OneSecondLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            OnOneSecond();
        }
    }

    private void OnOneSecond()
    {
        if (GameBeaten) {return;}
        TimeLeft--;
        clockCanvas.UpdateTimer(TimeLeft);
        if (TimeLeft <= 0)
        {
            StopCoroutine(timerCoroutine);
            TimeRanOut();

        }
    }
    private void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }


    private static GameMaster _instance;
    public static GameMaster Instance {get {return _instance;}}

    public FlickerLight lightBulb;
    public RadioScript radio;

    public Renderer cameraScreen;

    public GameObject monitorCover;

    public PasswordCanvas passwordCanvas;
    public RotatingDoorPuzzle rotatingDoorPuzzle;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    // thank you: https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern

    public int PuzzlesSolved;
    public bool GameBeaten = false;

    public Material cameraScreenActive;
    public Material cameraScreenInactive;
    public Material passwordScreenActive;


    void Start()
    {
        //in case you want to debug stuff, call Puzzle___Solved() in order here.
        StartEverything();
        timerCoroutine = StartCoroutine(OneSecondLoop());
    }
    // public void PuzzleCompleted()
    // {
    //     PuzzlesSolved++;

    //     Debug.Log("Puzzles solved: " + PuzzlesSolved);

    //     // Check game progression
    //     CheckGameProgress();
    // }

    // private void CheckGameProgress()
    // {
    //     if (PuzzlesSolved == 1)
    //     {
    //         Debug.Log("First puzzle completed!");
    //     }

    //     if (PuzzlesSolved == 3)
    //     {
    //         Debug.Log("Three puzzles completed!");
    //     }
    // }

    public void StartEverything()
    {
        // cover up stuff and make it dark
        // GameObject lightbulb_actual = GameObject.Find("Room Bulb");
        // lightbulb_actual.GetComponent<MeshRenderer>().enabled = false;
        // GameObject pointlight = GameObject.Find("Point Light Bulb");
        // pointlight.GetComponent<Light>().enabled = false;
        lightBulb.TurnOff();
        cameraScreen.material = cameraScreenInactive;
        //GameObject otherMonitorCover = GameObject.Find("CameraSystem_Cover");
        //otherMonitorCover.GetComponent<Renderer>().material = cameraScreenInactive;
    }

    /// <summary>
    /// This is called when the sliding puzzle is solved and should activate the "power" in the bunker.
    /// To do this im just gonna disable the point light in the props at the start, 
    /// </summary>
    public void PuzzleOneSolved()
    {
        // GameObject lightbulb_actual = GameObject.Find("Room Bulb");
        // lightbulb_actual.GetComponent<MeshRenderer>().enabled = true;
        // GameObject pointlight = GameObject.Find("Point Light Bulb");
        // pointlight.GetComponent<Light>().enabled = false;
        lightBulb.TurnOn();
        cameraScreen.material = passwordScreenActive;
        //GameObject otherMonitorCover = GameObject.Find("CameraSystem_Cover");
        //otherMonitorCover.GetComponent<Renderer>().material = passwordScreenActive;
        // monitorCover.GetComponent<MeshRenderer>().enabled = false;
        radio.StartRadio();

    }

    public void PuzzleTwoSolved()
    {
        cameraScreen.material = cameraScreenActive;
        rotatingDoorPuzzle.Activate();
        radio.StopRadio();
    }

    public void PuzzleThreeSolved()
    {
        GameBeaten = true;
    }

    public void TimeRanOut()
    {
        // code goes here...
        print("TIME WENT OFF OH MY GOD!!!");
    }
}
