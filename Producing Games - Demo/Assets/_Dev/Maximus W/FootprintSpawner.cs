using System.Collections;
using UnityEngine;

public class FootprintSpawner : MonoBehaviour
{
    public GameObject footprintPrefab;
    public float spawnInterval = 0.5f;
    public float footprintLifetime = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Adjust the tag as needed
        {
            StartCoroutine(SpawnFootprints());
        }
    }

    private IEnumerator SpawnFootprints()
    {
        while (true)
        {
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * 2f; // Adjust radius as needed
            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            GameObject footprint = Instantiate(footprintPrefab, randomPosition, randomRotation);
            yield return new WaitForSeconds(spawnInterval);
            Destroy(footprint, footprintLifetime);
        }
    }
}
