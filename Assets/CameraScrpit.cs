using UnityEngine;

public class CameraScrpit : MonoBehaviour
{
    public Transform target; // The character that the camera will follow
    public float smoothSpeed = 10f; // The speed at which the camera will follow the character
    public float upwardMovementSpeed = 1f; // The initial speed at which the camera moves upwards when not following the character
    public float upwardMovementAcceleration = 0.1f; // The rate at which the upward movement speed increases over time
    public float maxUpwardMovementSpeed = 5f; // The maximum speed at which the camera can move upwards

    private Vector3 lastTargetPosition; // The last recorded position of the target
    private float minY; // The minimum y-coordinate for the camera

    void Start()
    {
        // Calculate the minY value based on the middle of the screen
        minY = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).y;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void LateUpdate()
    {
        // Check if the target (player character) is not null
        if (target != null)
        {
            // Check if the target is above the camera's current position
            if (target.position.y > transform.position.y)
            {
                // Move the camera to follow the target
                Vector3 desiredPosition = transform.position;
                desiredPosition.y = target.position.y;
                transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            }
            else
            {
                // Accelerate the upward movement speed
                upwardMovementSpeed = Mathf.Min(upwardMovementSpeed + upwardMovementAcceleration * Time.deltaTime, maxUpwardMovementSpeed);

                // Move the camera upwards at the updated speed when not following the target
                transform.Translate(Vector3.up * upwardMovementSpeed * Time.deltaTime);
            
                // Clamp camera position to minY
                transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y, minY), transform.position.z);
            }
            
            // Store the current position of the target for the next frame
            lastTargetPosition = target.position;
        }
    }

}