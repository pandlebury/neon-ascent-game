using UnityEngine;

public class BrokenObstacle : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody2D rb;
    public float distanceBelowCameraToDestroy = 2f; // Distance below the camera to destroy the obstacle
    public float gravityScaleOnTrigger = 1.0f;
    private bool hasCollided = false; // Flag to track if the obstacle has collided

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
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

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check for null reference and then the tag
        if (other != null && other.CompareTag("Player") && !hasCollided)
        {
            // Set Rigidbody2D to dynamic
            rb.isKinematic = false;

            // Set the gravity scale
            rb.gravityScale = gravityScaleOnTrigger;

            // Set collided flag
            hasCollided = true;
        }
    }
}
