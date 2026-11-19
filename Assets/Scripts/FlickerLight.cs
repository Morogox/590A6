
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickerLight : MonoBehaviour
{
    public Renderer bulbRenderer;

    [Header("Gap between flickers (seconds)")]
    public float minGap = 2f;
    public float maxGap = 8f;

    [Header("Flicker length (seconds)")]
    public float minDuration = 0.2f;
    public float maxDuration = 1.5f;

    [Header("Flicker intensity")]
    public float minBrightness = 0.1f;
    public float stepTime = 0.04f;

    private Light lightComp;
    private float baseIntensity;
    private Material bulbMat;

    public AudioSource bulbHum;
    public AudioSource bulbFlicker;
    public AudioSource bulbExplode;

    [Header("Light State")]
    public bool on = false;

    private Coroutine flickerCoroutine;

    void Awake()
    {
        lightComp = GetComponent<Light>();
        baseIntensity = lightComp.intensity;

        if (bulbRenderer != null)
            bulbMat = bulbRenderer.material;

        SetBrightness(0f);
        bulbFlicker.Play();
        bulbFlicker.Pause();
    }

    public void ToggleLight()
    {
        if (on)
            TurnOff(false);
        else
            TurnOn();
    }

    public void TurnOn()
    {
        if (on) return;

        on = true;

        if (flickerCoroutine != null)
            StopCoroutine(flickerCoroutine);

        flickerCoroutine = StartCoroutine(FlickerLoop());
        bulbHum.Play();
        
    }

    public void TurnOff(bool Explode)
    {
        if (Explode)
        {
            bulbExplode.Play();
        }
        on = false;

        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
        }

        SetBrightness(0f);
        bulbHum.Stop();
        bulbFlicker.Stop();
        
    }

    IEnumerator FlickerLoop()
    {
        // Immediately begin with a flicker
        while (on)
        {
            float flickerEnd = Time.time + Random.Range(minDuration, maxDuration);
            float depth = Random.Range(0.3f, 1f);

            bulbFlicker.UnPause();

            while (on && Time.time < flickerEnd)
            {
                SetBrightness(
                    Mathf.Lerp(1f, Random.Range(minBrightness, 1f), depth)
                );

                yield return new WaitForSeconds(stepTime);
            }

            bulbFlicker.Pause();

            if (!on) break;

            SetBrightness(1f);

            yield return new WaitForSeconds(Random.Range(minGap, maxGap));
        }

        flickerCoroutine = null;
    }

    void SetBrightness(float b)
    {
        lightComp.intensity = baseIntensity * b;

        if (bulbMat != null)
            bulbMat.SetFloat("_Brightness", b);
    }
}
