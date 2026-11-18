using UnityEngine;

public class MorseCodeChildRelay : MonoBehaviour
{


    public int keyID;
    public PasswordCanvas parentScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {}

    public void OnActivation()
    {
        if (parentScript != null)
        {
            parentScript.EnterANumber(keyID);
        }
    }
}
