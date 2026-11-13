using System;
using Unity.VisualScripting;
using UnityEngine;

public class SlidingPuzzleScript : MonoBehaviour
{

    // im sure there is a better way of doing this, but idk and it's too late to care atp. 
    public GameObject square1;
    public GameObject square2;
    public GameObject square3;
    public GameObject square4;
    public GameObject square5;
    public GameObject square6;
    public GameObject square7;
    public GameObject square8;

    private int[][] data = new int[][]
    {
        new int[] {1, 2, 3},
        new int[] {4, 5, 6},
        new int[] {7, 8, 0}
    };

/// <summary>
/// tells if the puzzle has been solved, starts as false and once true, never goes back.
/// </summary>
    public bool puzzleSolved = false;
    int emptySpot = -1;

/// <summary>
/// Call this to reset the puzzle back to default,
/// </summary>
    public void SetPuzzle() {
        data = new int[][]
        {
            new int[] {3, 4, 5},
            new int[] {7, 2, 8},
            new int[] {1, 6, 0}
        };
    }

/// <summary>
/// returns if the current data is equal to the solved solution
/// </summary>
/// <returns>true or false based on if it works :D</returns>
    bool IsPuzzleSolved()
    {
        for (int i=1; i<9; i++)
        {
            if (GetValueFromSpot(i) != i) {return false;}
        }

        if (GetValueFromSpot(9) != 0) {return false;}
        return true;
    }

    public void printData()
    {
        // for (int i = 0; i < 3; i++)
        //     {
        //         for (int j = 0; j < 3; j++)
        //         {
        //             print(data[i][j]);
        //             print("");
        //         }


        //         print("");
        //     }
        //     print("");

        Debug.Log(Convert.ToString(data[0][0]) + Convert.ToString(data[0][1]) + Convert.ToString(data[0][2]) + "\n"
                + Convert.ToString(data[1][0]) + Convert.ToString(data[1][1]) + Convert.ToString(data[1][2]) + "\n"
                + Convert.ToString(data[2][0]) + Convert.ToString(data[2][1]) + Convert.ToString(data[2][2]));
    }

    int GetValueFromSpot(int spot)
    {
        int col = (spot - 1) % 3;
        int row = (spot - 1) / 3;
        return data[row][col];
    }

    int GetSpotFromValue(int value)
    {
        for (int row=0; row<3; row++)
        {
            for (int col=0; col<3; col++)
            {
                if (data[row][col] == value)
                {
                    return (row * 3) + 1 + col;
                }
            }
        }
        return -1;
    }


    /// <summary>
    /// Set a value at a spot in the data
    /// </summary>
    /// <param name="spot">The spot you want to set</param>
    /// <param name="value">The value you want to put at the spot</param>
    void SetValueAtSpot(int spot, int value)
    {
        int col = (spot - 1) % 3;
        int row = (spot - 1) / 3;
        data[row][col] = value;
    }


    /// <summary>
    /// uses data, and returns an int from 1-9 to see which spot is the empty one (0)
    /// </summary>
    /// <returns></returns>
    int FindEmptySpot() // spot 1-9
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (data[i][j] == 0)
                {
                    return (i * 3) + 1 + j;
                }
            }
        }
        // if something goes really wrong
        return -1;
    }


    /// <summary>
    /// using the given empty spot, returns an aray of valid spots that it can swap with
    /// </summary>
    /// <param name="emptySpot">empty spot of the data</param>
    /// <returns>returns a size 4 array in the order of [top, left, bottom, right] of the valid spot, -1 denotes that the spot is invalid</returns>
    int[] FindValidMoves(int emptySpot)
    {
        int[] validSpots = new int[] {-1, -1, -1, -1};
        int col = (emptySpot - 1) % 3;
        int row = (emptySpot - 1) / 3;


        // top num
        if (row - 1 >= 0) {validSpots[0] = ((row - 1) * 3) + col + 1;} // top num
        if (col - 1 >= 0) {validSpots[1] = (row * 3) + col;} // left num
        if (row + 1 < 3) {validSpots[2] = ((row + 1) * 3) + col + 1;} // bottom num
        if (col + 1 < 3) {validSpots[3] = (row * 3) + col + 2;} // right num


        return validSpots;


    }


    /// <summary>
    /// given a spot on the board, returns if it is a valid move
    /// </summary>
    /// <param name="spot">a given spot on the board</param>
    /// <returns>a boolean value representing if the move is valid</returns>
    bool CheckValidMove(int spot)
    {
        emptySpot = FindEmptySpot();
        int[] validMoves = FindValidMoves(emptySpot);


        for (int i = 0; i < 4; i++)
        {
            if (validMoves[i] == spot) {return true;}
        }


        return false;
    }


    /// <summary>
    /// given a spot, check if it's valid and then swap the empty spot and the spot
    /// </summary>
    /// <param name="spot">given spot</param>
    public void DoMove(int spot)
    {
        if (CheckValidMove(spot) && !puzzleSolved) // if the spot is a valid move
        {
            int temp = GetValueFromSpot(spot); // save the value of the spot you're clicking on
            int newNotEmptySpot = emptySpot; // save the spot of the empty spot
            SetValueAtSpot(newNotEmptySpot, temp);
            SetValueAtSpot(spot, 0);  
            SetSpotPositions();
            emptySpot = FindEmptySpot();

            if (IsPuzzleSolved())
            {
                puzzleSolved = true;
                PuzzleIsSolved();
                
            }

        }
    }

    public void DoMoveFromValue(int value)
    {
        DoMove(GetSpotFromValue(value));
    }

    public void SetSpotPositions()
    {
        for (int i=1; i<=9; i++)
        {
            SetSpotPosition(i, GetValueFromSpot(i));
        }
    }

/// <summary>
/// Make a certain spot have a certain value, this is the transform of it
/// </summary>
/// <param name="spot">Where you want the value to be</param>
/// <param name="value">The value you want to be</param>
    public void SetSpotPosition(int spot, int value)
    {
        int col = (spot - 1) % 3;
        int row = (spot - 1) / 3;
        Vector3 newPos = new Vector3(0, (float)(row * -0.111), (float)(col * -0.114));
        
        switch (value)
        {
            case 1:{
                float old_x = square1.transform.localPosition.x;
                newPos.x = old_x;
                square1.transform.localPosition = newPos;
                break;}
            case 2:{
                float old_x = square2.transform.localPosition.x;
                newPos.x = old_x;
                square2.transform.localPosition = newPos;
                break;}
            case 3:{
                float old_x = square3.transform.localPosition.x;
                newPos.x = old_x;
                square3.transform.localPosition = newPos;
                break;}
            case 4:{
                float old_x = square4.transform.localPosition.x;
                newPos.x = old_x;
                square4.transform.localPosition = newPos;
                break;}
            case 5:{
                float old_x = square5.transform.localPosition.x;
                newPos.x = old_x;
                square5.transform.localPosition = newPos;
                break;}
            case 6:{
                float old_x = square6.transform.localPosition.x;
                newPos.x = old_x;
                square6.transform.localPosition = newPos;
                break;}
            case 7:{
                float old_x = square7.transform.localPosition.x;
                newPos.x = old_x;
                square7.transform.localPosition = newPos;
                break;}
            case 8:{
                float old_x = square8.transform.localPosition.x;
                newPos.x = old_x;
                square8.transform.localPosition = newPos;
                break;}

        }

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetPuzzle();
        SetSpotPositions();
    }

    // Update is called once per frame
    void Update() {}


    /// <summary>
    /// THIS WILL BE CALLED WHEN THE PUZZLE IS CONSIDERED SOLVED!
    /// </summary>
    private void PuzzleIsSolved()
    {
        // PUT ANY AND ALL CODE FOR WHEN THIS PUZZLE IS SOLVED HERE!
    }
}
