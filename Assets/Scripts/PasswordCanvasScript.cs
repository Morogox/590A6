using System;
using GLTFast.Schema;
using Oculus.Interaction;
using TMPro;
using UnityEngine;

public class PasswordCanvas : MonoBehaviour
{

    private int[] REALPASSWORD = {1, 2, 2, 6};
    public int[] passwordEntry = {-1, -1, -1, -1};
    public int index = 0;
    private bool solved = false;

    public GameObject number1;
    public GameObject number2;
    public GameObject number3;
    public GameObject number4;
    public GameObject passwordBackground;

    
    private TMP_Text[] numberTexts = new TMP_Text[4];
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numberTexts[0] = number1.GetComponent<TMP_Text>();
        numberTexts[1] = number2.GetComponent<TMP_Text>();
        numberTexts[2] = number3.GetComponent<TMP_Text>();
        numberTexts[3] = number4.GetComponent<TMP_Text>();
        ResetNumbers();

    }

    // Update is called once per frame
    void Update() {}

/// <summary>
/// Dont use this one anymore, aka
/// </summary>
/// <param name="number"></param>
/// <param name="spot"></param>
    private void UpdateNumber(int number, int spot)
    {
        if (number == -1) {numberTexts[spot].SetText("_");}
        else {
            numberTexts[spot].SetText(number.ToString());
        }
        passwordEntry[spot] = number;
    }


    /// <summary>
    /// This one is only called when there are 0-3 numbers in the text field
    /// </summary>
    /// <param name="number"></param>
    private void EnterNumber(int number)
    {
        if (index > 3) {return;}
        UpdateNumber(number, index);
        index++;
    }

    private void ResetNumbers()
    {
        for (int i = 0; i < 4; i++)
        {
            numberTexts[i].SetText("_");
            passwordEntry[i] = -1;
        }
        index = 0;

    }

    private void DeleteLastNumber()
    {
        if (index <= 0) {return;}
        UpdateNumber(-1, index-1);
        index--;
    }

    public void UpdateAllNumbers()
    {
        for (int i = 0; i < 4; i++)
        {
            if (passwordEntry[i] == -1)
            {
                numberTexts[i].SetText("_");
            } 
            else {
                numberTexts[i].SetText(passwordEntry[i].ToString());
            }
        }
    }

    private bool TryPassword()
    {
        if (index != 4) {return false;}
        for (int i = 0; i < 4; i++)
        {
            if (REALPASSWORD[i] != passwordEntry[i]) {return false;}
        }
        return true;
    }

    private void PuzzleSolved()
    {
        ResetNumbers();
        numberTexts[0].SetText("t");
        numberTexts[1].SetText("r");
        numberTexts[2].SetText("u");
        numberTexts[3].SetText("e");
        GameMaster.Instance.PuzzleTwoSolved();
    }

    public void EnterANumber(int keyID)
    {
        if (solved) {return;}
        //0-9 are the same, 10 - enter key, 11 - delete key
        print(keyID);

        if (0 <= keyID && keyID < 10) // number entry
        {
            EnterNumber(keyID);
        }

        if (keyID == 10)
        {
            if (TryPassword())
            {
                solved = true;
                PuzzleSolved();
            } else {
                ResetNumbers();
            }
        }

        if (keyID == 11)
        {
            DeleteLastNumber();
        }

    }
}
