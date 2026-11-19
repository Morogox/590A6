using UnityEngine;
using TMPro;

public class ClockCanvas : MonoBehaviour
{

    public TMP_Text timer;
    
    public void UpdateTimer(int secondsleft)
    {

        if (secondsleft < 60)
        {
            timer.text = secondsleft.ToString();
            return;
        }

        int minutesLeft = secondsleft / 60;
        int secondsLeft = secondsleft % 60;

        timer.text = minutesLeft.ToString() + " : " + secondsLeft.ToString();
    }


}
