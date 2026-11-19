using System;
using UnityEngine;
using Oculus.Interaction;
using UnityEngine.Experimental.GlobalIllumination;
using System.Collections;

public class RotatingDoorPuzzle : MonoBehaviour
{

    // this is how im storing it, - indicates that the turn is counter clockwise, + indicates that it's clockwise.
    private int turn1 = -5; // split into two cameras being -2 for 1, and -3 for the second one
    private int turn2 = 1;
    private int turn3 = -5;

    public bool PuzzleActive = false;

    public GameObject WheelVisuals;
    public Material InactiveMaterial;
    public Material ActiveMaterial;
    public Material SolvedMaterial;
    public Light WheelVisualsLight;

    public AudioSource doorUnlock;
    public AudioSource ladder;
    public enum Status
    {
        Turn1,
        Turn2,
        Turn3,
        Solved
    }


    private Status puzzleStatus = Status.Turn1;

    private int amountTurnsLeft = 0;

    public void Activate()
    {
        WheelVisuals.GetComponent<Renderer>().material = ActiveMaterial;
        WheelVisualsLight.color = Color.yellow;
        ResetPuzzle();
        PuzzleActive = true;
    }

    public void Deactivate()
    {
        WheelVisuals.GetComponent<Renderer>().material = InactiveMaterial;
        WheelVisualsLight.color = Color.red;
        ResetPuzzle();
        PuzzleActive = false;

    }

    private void ResetPuzzle()
    {
        amountTurnsLeft = -turn1;
        puzzleStatus = Status.Turn1;
    }

    private void PuzzleSolved()
    {
        print("PUZZLESOLVED() called!");
        GameMaster.Instance.PuzzleThreeSolved();
        WheelVisuals.GetComponent<Renderer>().material = SolvedMaterial;
        WheelVisualsLight.color = Color.green;
        StartCoroutine(PlayUnlockSounds());
    }

    private IEnumerator PlayUnlockSounds()
    {
        doorUnlock.Play();

        yield return new WaitForSecondsRealtime(doorUnlock.clip.length);

        ladder.Play();
    }

    /// <summary>
    /// Private method so we can just use TurnLeft/RightOnce
    /// </summary>
    /// <param name="direction">-1 for counter, 1 for non-counter</param>
    private void TurnOnce(int direction)
    {
        if (!PuzzleActive) {return;}
        if (puzzleStatus == Status.Solved) {return;}

        switch (puzzleStatus)
        {
            case Status.Turn1:
                if (direction == -1) // left
                {
                    amountTurnsLeft--;
                    if (amountTurnsLeft == 0)
                    {
                        puzzleStatus = Status.Turn2;
                        amountTurnsLeft = -turn2;
                    }
                } 
                else // right
                {
                    amountTurnsLeft = -turn1;
                }
                break;
            case Status.Turn2:
                if (direction == 1) // right
                {
                    amountTurnsLeft++;
                    if (amountTurnsLeft == 0)
                    {
                        puzzleStatus = Status.Turn3;
                        amountTurnsLeft = -turn3;
                    }
                }
                else // left
                {
                    amountTurnsLeft = -turn1;
                    puzzleStatus = Status.Turn1;
                }
                break;
            case Status.Turn3:
                if (direction == -1) // left
                {
                    amountTurnsLeft--;
                    if (amountTurnsLeft == 0)
                    {
                        puzzleStatus = Status.Solved;
                        amountTurnsLeft = -1;
                        PuzzleSolved();
                    }
                }
                else // right
                {
                    amountTurnsLeft = -turn1;
                    puzzleStatus = Status.Turn1;
                }
                break;
        }
    }


    public void TurnLeftOnce() {TurnOnce(-1); PrintPuzzleStatus();}


    public void TurnRightOnce() {TurnOnce(1); PrintPuzzleStatus();}

    public void PrintPuzzleStatus()
    {
        print("Puzzle Status: " + puzzleStatus + '\n' + "Amount Turns Left: " + amountTurnsLeft);
    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
    }

}
