using UnityEngine;

public class SlidingPuzzleChildRelay : MonoBehaviour
{
    public int spotID;
    private SlidingPuzzleScript parentScript;
    

    void Start()
    {
        parentScript = GetComponentInParent<SlidingPuzzleScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (parentScript != null)
        {
            parentScript.DoMoveFromValue(spotID);
        }
    }
}
