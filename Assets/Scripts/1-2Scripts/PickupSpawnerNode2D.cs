using UnityEngine;

/// <summary>
/// SINGLE PURPOSE PRINCIPLE EXPLANATION: This script handles exactly one job:
/// managing the collection detection and timed respawning of a single pickup object.
/// </summary>
public class PickupSpawnerNode2D : MonoBehaviour
{
    [Header("Spawn Settings (Designer Configurable)")]
    [SerializeField] private GameObject pickupPrefabToSpawn;
    [SerializeField] private float respawnCooldownTime = 5f; // Seconds to wait before respawning

    private GameObject currentlySpawnedPickup;
    private float respawnTimer;
    private bool isWaitingToRespawn = false;

    private void Start()
    {
        // Generate the initial pickup right when the game starts
        SpawnNewPickupInstance();
    }

    private void Update()
    {
        // Check if our tracked pickup was collected (destroyed by the FloatingPickup2D script)
        if (!isWaitingToRespawn && currentlySpawnedPickup == null)
        {
            isWaitingToRespawn = true;
            respawnTimer = respawnCooldownTime; // Begin the countdown
            Debug.Log($"Pickup collected! Starting {respawnCooldownTime} second respawn cooldown.");
        }

        // UAT REQUIREMENT: Data updates every frame draw and must be framerate independent
        if (isWaitingToRespawn)
        {
            respawnTimer -= Time.deltaTime; // Decrement safely across variable hardware speeds

            if (respawnTimer <= 0f)
            {
                SpawnNewPickupInstance();
            }
        }
    }

    private void SpawnNewPickupInstance()
    {
        if (pickupPrefabToSpawn == null)
        {
            Debug.LogWarning($"Cannot spawn pickup on {gameObject.name} because the prefab slot is empty!");
            return;
        }

        // Instantiate the pickup prefab directly at this node's exact position coordinates
        currentlySpawnedPickup = Instantiate(pickupPrefabToSpawn, transform.position, Quaternion.identity);

        // Parent it to this node to keep the Unity Hierarchy clean and organized
        currentlySpawnedPickup.transform.SetParent(transform);

        // Reset tracking states
        isWaitingToRespawn = false;
    }

    // Optional: Visual anchor to easily locate the invisible spawner nodes in the Unity Editor Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.4f);
    }
}
