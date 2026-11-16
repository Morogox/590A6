
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public int puzzlesSolved = 0;

    public void PuzzleCompleted()
    {
        puzzlesSolved++;

        Debug.Log("Puzzles solved: " + puzzlesSolved);

        // Check game progression
        CheckGameProgress();
    }

    private void CheckGameProgress()
    {
        if (puzzlesSolved == 1)
        {
            Debug.Log("First puzzle completed!");
        }

        if (puzzlesSolved == 3)
        {
            Debug.Log("Three puzzles completed!");
        }
    }
}
