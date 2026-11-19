
using Oculus.Interaction.Locomotion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public string GetTimeTaken()
    {
        int timeTaken = 600 - TimeLeft;

        int minutes = timeTaken / 60;
        int seconds = timeTaken % 60;

        return $"{minutes:00}:{seconds:00}";
    }

    private static GameMaster _instance;
    public static GameMaster Instance {get {return _instance;}}

    public FlickerLight lightBulb;
    public RadioScript radio;

    public Renderer cameraScreen;

    public GameObject monitorCover;

    public SlidingPuzzleScript slidingPuzzle;
    public PasswordCanvas passwordCanvas;
    public RotatingDoorPuzzle rotatingDoorPuzzle;

    [Header("Screen Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 2f;

    

    [SerializeField] private List<TextMeshProUGUI> Texts;

    public AlienLightScript spookySpawn;

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
    public AudioSource doorKnocking;
    public Transform teleportCoords;
    public GameObject player;

    public AudioSource winter;


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
        //lightBulb.TurnOff();
        cameraScreen.material = cameraScreenInactive; 
        slidingPuzzle.Activate();
    }

    /// <summary>
    /// This is called when the sliding puzzle is solved and should activate the "power" in the bunker.
    /// To do this im just gonna disable the point light in the props at the start, 
    /// </summary>
    public void PuzzleOneSolved()
    {
        lightBulb.TurnOn();
        cameraScreen.material = passwordScreenActive;
        radio.StartRadio();
        passwordCanvas.Activate();
        slidingPuzzle.Deactivate();

    }

    public void PuzzleTwoSolved()
    {
        cameraScreen.material = cameraScreenActive;
        rotatingDoorPuzzle.Activate();
        radio.StopRadio();
        passwordCanvas.Deactivate();
    }

    public void PuzzleThreeSolved()
    {
        print("puzzlethreeSoled Called");
        GameBeaten = true;
        StopTimer();
        WinSequence();
    }

    public void TimeRanOut()
    {
        // code goes here...
        print("TIME WENT OFF OH MY GOD!!!");
        lightBulb.TurnOff(true);
        cameraScreen.material = cameraScreenInactive;
        radio.StopRadio();
        passwordCanvas.Deactivate();
        slidingPuzzle.Deactivate();
        rotatingDoorPuzzle.Deactivate();
        FailSequence();
    }


    public void WinSequence()
    {
        print("Win Sequence Called");
        StartCoroutine(WinSequenceRoutine());
    }
    private IEnumerator WinSequenceRoutine()
    {
        print("Coroutine started");
        DisablePlayerMovement();
        yield return StartCoroutine(FadeToBlack());
        player.transform.position = teleportCoords.position;
        yield return new WaitForSecondsRealtime(2f);
        lightBulb.TurnOff(false);
        winter.Play();
        yield return StartCoroutine(FadeFromBlack());
        yield return new WaitForSecondsRealtime(5f);
        spookySpawn.StartLightSequence();
        yield return new WaitForSecondsRealtime(25f);

        yield return StartCoroutine(FadeToBlack());
        string finalTime = GetTimeTaken();

        Texts[0].text = "ESCAPED";
        Texts[1].text = "Time taken: " + finalTime;
        Texts[0].gameObject.SetActive(true);
        Texts[1].gameObject.SetActive(true);

    }


    public void FailSequence()
    {
        StartCoroutine(FailSequenceRoutine());
    }

    private IEnumerator FailSequenceRoutine()
    {
        // Wait 15 seconds in complete silence
        yield return new WaitForSecondsRealtime(15f);

        doorKnocking.Play();

        yield return new WaitForSecondsRealtime(doorKnocking.clip.length);

        yield return StartCoroutine(FadeToBlack());

        Texts[0].text = "They found you!";
        Texts[1].text = "Failed to escape";
        Texts[0].gameObject.SetActive(true);
        Texts[1].gameObject.SetActive(true);
    }


    public IEnumerator FadeToBlack()
    {
        float elapsed = 0f;
        float startAlpha = fadeCanvasGroup.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            fadeCanvasGroup.alpha = Mathf.Lerp(
                startAlpha, 1f, elapsed / fadeDuration
            );

            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
    }

    public IEnumerator FadeFromBlack()
    {
        float elapsed = 0f;
        float startAlpha = fadeCanvasGroup.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            fadeCanvasGroup.alpha = Mathf.Lerp(
                startAlpha, 0f, elapsed / fadeDuration
            );

            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
    }


   

    public FirstPersonLocomotor playerLocomotor;

    public void DisablePlayerMovement()
    {
        playerLocomotor.DisableMovement();
    }

    public void EnablePlayerMovement()
    {
        playerLocomotor.EnableMovement();
    }
}

