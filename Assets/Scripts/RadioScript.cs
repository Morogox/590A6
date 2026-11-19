using UnityEngine;
using System.Collections;

public class RadioScript : MonoBehaviour
{
    public AudioSource radioStart;
    public AudioSource radioMessage;
    public float waitTime = 5f;
    private Coroutine radioCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartRadio()
    {
        radioCoroutine = StartCoroutine(RadioSequence());
    }

    public void StopRadio()
    {
        if (radioCoroutine != null)
        {
            StopCoroutine(radioCoroutine);
            radioCoroutine = null;
        }

        radioStart.Stop();
        radioMessage.Stop();
    }
    IEnumerator RadioSequence()
    {
        yield return new WaitForSeconds(waitTime);

        radioStart.Play();

        yield return new WaitForSeconds(radioStart.clip.length - 1);

        radioMessage.Play();
    }
}
