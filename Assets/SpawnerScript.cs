using UnityEngine;

[System.Serializable]
public class SpawnablePrefab
{
    public GameObject prefab;
    public float spawnChance;
}

public class SpawnerScript : MonoBehaviour
{
    public SpawnablePrefab[] spawnablePrefabs; // Array of spawnable prefabs with their spawn chances
    public float spawnInterval = 2f; // Time interval between each spawning
    public float spawnOffset = 1f; // Offset from the center of the screen where the points will be spawned
    public float spawnSpeed = 5f; // Speed at which the jumping points move downwards
    public float minSpawnDistance = 2f; // Minimum distance between each spawned point on the y-axis

    private float lastSpawnTime; // Time when the last jumping point was spawned
    private float lastSpawnY; // Y position of the last spawned jumping point

    void Start()
    {
        lastSpawnTime = Time.time;
        lastSpawnY = transform.position.y;
    }

    void Update()
    {
        // Check if it's time to spawn a new jumping point
        if (Time.time - lastSpawnTime >= spawnInterval)
        {
            SpawnJumpingPoint();
            lastSpawnTime = Time.time;
        }
    }

    void SpawnJumpingPoint()
    {
        // Calculate random x position within the screen bounds
        float randomX = Random.Range(-spawnOffset, spawnOffset);
        
        // Calculate spawn position at the top of the screen
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, 0f);

        // Adjust spawn position to ensure it's not too close to the last spawned point
        float newY = lastSpawnY + minSpawnDistance;
        spawnPosition.y = newY;

        // Randomly select a prefab to spawn based on spawn chances
        GameObject prefabToSpawn = SelectRandomPrefab();
        
        // Instantiate the selected prefab at the spawn position
        GameObject newJumpingPoint = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        
        // Set the parent of the jumping point to this spawner
        newJumpingPoint.transform.parent = transform;

        // Apply downward movement to the jumping point
        Rigidbody2D rb = newJumpingPoint.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.down * spawnSpeed;
        }

        // Update the last spawned y position
        lastSpawnY = newY;
    }

    GameObject SelectRandomPrefab()
    {
        float totalSpawnChance = 0f;

        // Calculate the total spawn chance
        foreach (var prefab in spawnablePrefabs)
        {
            totalSpawnChance += prefab.spawnChance;
        }

        // Generate a random number between 0 and totalSpawnChance
        float randomValue = Random.Range(0f, totalSpawnChance);

        // Iterate through the spawnable prefabs to find which one to spawn
        foreach (var prefab in spawnablePrefabs)
        {
            // If the random value is less than the current prefab's spawn chance, spawn this prefab
            if (randomValue < prefab.spawnChance)
            {
                return prefab.prefab;
            }
            // Otherwise, subtract the spawn chance of the current prefab from the random value
            else
            {
                randomValue -= prefab.spawnChance;
            }
        }

        // Return the last prefab if no other prefab is selected
        return spawnablePrefabs[spawnablePrefabs.Length - 1].prefab;
    }
}
