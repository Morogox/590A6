
using UnityEngine;

public class DoorKnockAudio : MonoBehaviour
{
    [SerializeField] private AudioSource lightKnock;
    [SerializeField] private AudioSource urgentKnock;
    [SerializeField] private AudioSource loudBangs;

    public void PlayLightKnock()
    {
        lightKnock.Play();
    }

    public void PlayUrgentKnock()
    {
        urgentKnock.Play();
    }

    public void PlayLoudBangs()
    {
        loudBangs.Play();
    }
}
