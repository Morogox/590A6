
using System.Collections;
using UnityEngine;

public class LanternLight : MonoBehaviour
{
    [SerializeField] private Light lanternLight;
    [SerializeField] private float targetIntensity = 0.1f;
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private bool startOn = false;

    private bool isOn;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        isOn = startOn;
        lanternLight.intensity = isOn ? targetIntensity : 0f;
    }

    private void Start()
    {
        isOn = startOn;
        lanternLight.intensity = isOn ? targetIntensity : 0f;
    }

    public void ToggleLight()
    {
        isOn = !isOn;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        float target = isOn ? targetIntensity : 0f;
        fadeCoroutine = StartCoroutine(FadeLight(target));
    }

    private IEnumerator FadeLight(float target)
    {
        float startIntensity = lanternLight.intensity;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / fadeDuration;
            lanternLight.intensity = Mathf.Lerp(startIntensity, target, t);

            yield return null;
        }

        lanternLight.intensity = target;
        fadeCoroutine = null;
    }
}
