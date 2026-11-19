using UnityEngine;
using System.Collections;

public class AlienLightScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform player;
    public GameObject lightPrefab;

    public float minDistance = 12f;
    public float maxDistance = 12f;

    public float minHeight = 2f;
    public float maxHeight = 5f;

    public float crescendoDuration = 25f;
    public int lightCount = 40;
    public AudioSource crescendo;


    void Start()
    {
        //transform.LookAt(player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void StartLightSequence()
    {
        crescendo.Play();
        StartCoroutine(SpawnLights());
        
    }

    private IEnumerator SpawnLights()
    {
        float elapsed = 0f;
        int spawned = 0;

        while (elapsed < crescendoDuration && spawned < lightCount)
        {
            // Spawn a light
            SpawnOneLight();
            spawned++;

            // Exponential acceleration
            float progress = elapsed / crescendoDuration;
            float delay = Mathf.Lerp(1.5f, 0.05f, progress * progress);

            yield return new WaitForSeconds(delay);
            elapsed += delay;
        }
    }



    private void SpawnOneLight()
    {
        float angle = Random.Range(0f, 360f);

        Vector3 direction = new Vector3(
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0f,
            Mathf.Cos(angle * Mathf.Deg2Rad)
        );

        float distance = Random.Range(minDistance, maxDistance);

        Vector3 spawnPosition = player.position + direction * distance;

        spawnPosition.y += Random.Range(minHeight, maxHeight);

        Quaternion rotation = Quaternion.LookRotation(
            player.position - spawnPosition
        );

        Instantiate(lightPrefab, spawnPosition, rotation);
    }

}
