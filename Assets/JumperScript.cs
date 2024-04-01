using UnityEngine;

public class JumperScript : MonoBehaviour
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
    public float jumpForce = 10f; // Adjustable jump force

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = collision.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                // Apply positive y-axis velocity to make the player jump
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
            }
        }
    }
}
