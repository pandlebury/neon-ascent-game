using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    private Camera mainCamera;
    public float distanceBelowCameraToDestroy = 2f; // Distance below the camera to destroy the obstacle

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Calculate the threshold position below the camera to destroy the obstacle
        float thresholdY = mainCamera.transform.position.y - distanceBelowCameraToDestroy;

        if (transform.position.y < thresholdY)
        {
            // Destroy the obstacle if it's below the camera plus the specified distance
            Destroy(gameObject);
        }
    }
}
