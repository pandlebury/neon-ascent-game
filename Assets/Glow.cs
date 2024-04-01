using UnityEngine;

public class Glow : MonoBehaviour
{
    public GameObject objectToSpawn; // Assign the prefab in the inspector

    // Method to spawn another GameObject
    public void SpawnObject()
    {
        if (objectToSpawn != null)
        {
            Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            Debug.Log("Object spawned by platform.");
        }
        else
        {
            Debug.LogError("No object to spawn assigned!");
        }
    }
    
}
