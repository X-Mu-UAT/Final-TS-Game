using UnityEngine;
using System.Collections.Generic; // Required for using Lists

/// <summary>
/// SINGLE PURPOSE PRINCIPLE EXPLANATION: This script has one sole responsibility: 
/// managing the endless generation and cleanup of dropping hazard objects over time. 
/// It does not process player input, handle scoring, or evaluate health damage.
/// </summary>
public class EndlessHazardSpawner : MonoBehaviour
{
    [Header("Spawning Rates (Designer Configuration)")]
    [SerializeField] private GameObject hazardPrefabToSpawn;
    [SerializeField] private float timeBetweenSpawns = 1.5f;
    [SerializeField] private float spawnWidthRange = 6f; // Left/Right variation boundary

    [Header("Automatic Performance Cleanup")]
    [SerializeField] private float autoDestroyYThreshold = -10f; // Y position where hazards vanish

    // UAT REQUIREMENT: Use at least one List
    // Keeps active tracking records of all spawned hazard objects currently alive in the scene
    private List<GameObject> activeHazardsList = new List<GameObject>();

    private float spawnTimer;

    private void Start()
    {
        // Initialize the timer so a hazard drops shortly after the level starts
        spawnTimer = timeBetweenSpawns;
    }

    private void Update()
    {
        // UAT REQUIREMENT: At least one piece of data changes every frame draw and must be framerate independent.
        // The spawnTimer ticks down using Time.deltaTime, ensuring spawning speeds are identical on all computers.
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnHazardItem(); // This line calls the method added below
            spawnTimer = timeBetweenSpawns;
        }

        for (int i = activeHazardsList.Count - 1; i >= 0; i--)
        {
            GameObject currentHazard = activeHazardsList[i];

            // If a hazard has been destroyed elsewhere or fell past the map's floor boundary, clean it up
            if (currentHazard == null)
            {
                activeHazardsList.RemoveAt(i);
            }
            else if (currentHazard.transform.position.y < autoDestroyYThreshold)
            {
                activeHazardsList.RemoveAt(i);
                Destroy(currentHazard);
            }
        }
    }

    /// <summary>
    /// THIS WAS THE MISSING METHOD BLOCK: Inserts the hazard prefab into the game world coordinates.
    /// </summary>
    private void SpawnHazardItem()
    {
        if (hazardPrefabToSpawn == null) return;

        // Calculate a random horizontal offset so hazards drop randomly across a designated area
        float randomXOffset = Random.Range(-spawnWidthRange / 2f, spawnWidthRange / 2f);
        Vector3 spawnPosition = transform.position + new Vector3(randomXOffset, 0f, 0f);

        // Instantiate the hazard into the level
        GameObject newHazard = Instantiate(hazardPrefabToSpawn, spawnPosition, Quaternion.identity);

        // Add the newly created object to our tracking List container
        activeHazardsList.Add(newHazard);
    }

    // Optional: Visualizes the spawning zone boundary lines directly within the Unity Editor Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 leftBound = transform.position + new Vector3(-spawnWidthRange / 2f, 0f, 0f);
        Vector3 rightBound = transform.position + new Vector3(spawnWidthRange / 2f, 0f, 0f);
        Gizmos.DrawLine(leftBound, rightBound);
    }
}
