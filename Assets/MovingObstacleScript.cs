using UnityEngine;

public class MovingObstacleScript : MonoBehaviour
{
    private Camera mainCamera;
    public float distanceBelowCameraToDestroy = 2f; // Distance below the camera to destroy the obstacle
    public float minMoveSpeed = 1f; // Minimum movement speed
    public float maxMoveSpeed = 5f; // Maximum movement speed
    public float horizontalSpawnOffset = 1f; // Offset from the center of the screen where the obstacles will be spawned

    private float currentMoveSpeed; // Current movement speed
    private float currentDirection; // Current direction of movement (-1 for left, 1 for right)

    void Start()
    {
        mainCamera = Camera.main;
        
        // Initialize movement speed and direction
        currentMoveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        currentDirection = Random.Range(-1f, 1f) > 0 ? 1f : -1f;

        // Ensure that the obstacles are spawned within the screen bounds
        ClampPositionToScreen();
    }

    void Update()
    {
        // Calculate the threshold position below the camera to destroy the obstacle
        float thresholdY = mainCamera.transform.position.y - distanceBelowCameraToDestroy;

        // Move the obstacle horizontally
        transform.Translate(Vector3.right * currentDirection * currentMoveSpeed * Time.deltaTime);

        // Check if the obstacle is below the camera plus the specified distance
        if (transform.position.y < thresholdY)
        {
            // Destroy the obstacle
            Destroy(gameObject);
        }

        // Check if the obstacle hits the screen borders and change direction
        if (transform.position.x <= -mainCamera.orthographicSize * mainCamera.aspect ||
            transform.position.x >= mainCamera.orthographicSize * mainCamera.aspect)
        {
            currentDirection *= -1f; // Reverse direction
        }

        // Randomly change speed
        if (Random.Range(0f, 1f) < 0.01f) // Adjust this probability as needed
        {
            currentMoveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed); // Change speed
        }
    }

    void ClampPositionToScreen()
    {
        // Clamp the position of the obstacle to ensure it is within the screen bounds
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -mainCamera.orthographicSize * mainCamera.aspect + horizontalSpawnOffset, mainCamera.orthographicSize * mainCamera.aspect - horizontalSpawnOffset);
        transform.position = clampedPosition;
    }
}
