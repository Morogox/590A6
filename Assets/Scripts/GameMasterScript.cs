
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    private static GameMaster _instance;
    public static GameMaster Instance {get {return _instance;}}

    public FlickerLight lightBulb;
    public RadioScript radio;

    public Renderer cameraScreen;
    public GameObject monitorCover;


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

    public Material cameraScreenActive;
    public Material cameraScreenInactive;


    void Start()
    {
        //in case you want to debug stuff, call Puzzle___Solved() in order here.
        StartEverything();
    }
    public void PuzzleCompleted()
    {
        PuzzlesSolved++;

        Debug.Log("Puzzles solved: " + PuzzlesSolved);

        // Check game progression
        CheckGameProgress();
    }

    private void CheckGameProgress()
    {
        if (PuzzlesSolved == 1)
        {
            Debug.Log("First puzzle completed!");
        }

        if (PuzzlesSolved == 3)
        {
            Debug.Log("Three puzzles completed!");
        }
    }

    public void StartEverything()
    {
        // cover up stuff and make it dark
        //GameObject lightbulb = GameObject.Find("Bulb");
        //lightbulb.GetComponent<MeshRenderer>().enabled = false;
        //GameObject pointlight = GameObject.Find("Point Light");
        //pointlight.GetComponent<Light>().enabled = false;
        cameraScreen.material = cameraScreenInactive;
        GameObject otherMonitorCover = GameObject.Find("CameraSystem_Cover");
        monitorCover.GetComponent<MeshRenderer>().enabled = true;
    }

    /// <summary>
    /// This is called when the sliding puzzle is solved and should activate the "power" in the bunker.
    /// To do this im just gonna disable the point light in the props at the start, 
    /// </summary>
    public void PuzzleOneSolved()
    {
        lightBulb.ToggleLight();
        cameraScreen.material = cameraScreenActive;
        monitorCover.GetComponent<MeshRenderer>().enabled = false;
        radio.StartRadio();

    }

    public void PuzzleTwoSolved()
    {
        radio.StopRadio();
    }

    public void PuzzleThreeSolved()
    {
        
    }
}
