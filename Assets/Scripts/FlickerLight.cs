using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickerLight : MonoBehaviour
{
    public Renderer bulbRenderer;          // drag your bulb mesh here

    [Header("Gap between flickers (seconds)")]
    public float minGap = 2f;
    public float maxGap = 8f;

    [Header("Flicker length (seconds)")]
    public float minDuration = 0.2f;
    public float maxDuration = 1.5f;

    [Header("Flicker intensity")]
    public float minBrightness = 0.1f;     // darkest a flicker can get
    public float stepTime = 0.04f;         // how fast it jitters

    Light lightComp;
    float baseIntensity;
    Material bulbMat;

    void Awake()
    {
        lightComp = GetComponent<Light>();
        baseIntensity = lightComp.intensity;
        if (bulbRenderer != null) bulbMat = bulbRenderer.material;
    }

    void OnEnable() { StartCoroutine(FlickerLoop()); }

    IEnumerator FlickerLoop()
    {
        while (true)
        {
            SetBrightness(1f);
            yield return new WaitForSeconds(Random.Range(minGap, maxGap));

            float flickerEnd = Time.time + Random.Range(minDuration, maxDuration);
            float depth = Random.Range(0.3f, 1f);   // subtle vs. deep stutter

            while (Time.time < flickerEnd)
            {
                SetBrightness(Mathf.Lerp(1f, Random.Range(minBrightness, 1f), depth));
                yield return new WaitForSeconds(stepTime);
            }
        }
    }

    void SetBrightness(float b)
    {
        lightComp.intensity = baseIntensity * b;
        if (bulbMat != null) bulbMat.SetFloat("_Brightness", b);
    }
}
