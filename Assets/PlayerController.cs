using System.Globalization;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private ControlOptionsManager.ControlType controlType;
    private SpriteRenderer whiteSquareSpriteRenderer;
    private OptionsManager optionsManager; // Reference to OptionsManager
    private Camera mainCamera;
    public float moveSpeed = 5f; // Speed of horizontal movement
    public float jumpForce = 10f; // Force applied when jumping
    public float moreJumpForce = 5f; // Additional force applied when jumping from Jumper
    public Transform groundCheck; // Transform representing the position for checking if the player is grounded
    public LayerMask groundLayer; // LayerMask for defining what is considered ground
    public LayerMask obstacleLayer; // LayerMask for defining what is considered obstacles
    public float autoJumpDelay = 1f; // Delay in seconds before automatic jump occurs
    private AdManager adManager;

    public GameObject RedSquare;
    public GameObject BlueSquare;
    public GameObject WhiteSquare;
    public GameObject GreenSquare;
    public GameObject NeonSquare2;

    bool isRedSquareActive = true;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float lastGroundedTime;
    private bool isFalling; // Flag to indicate whether the player is falling
    private float highestYPosition; // Highest Y position reached by the player
    private int points;
    public AudioSource jumpSound;
    public AudioSource JumperSound;

    public GameObject jumpParticlesPrefab;
    //public GameObject gameOverScreen;
    public GameObject gameOverScreenPrefab;
    public int StarsCollected;
    public int TotalStarsForShop { get; private set; }
    public float minYPosition;
    public float finalScore;
    public float tiltSensitivity = 2f;


    public string starscollected => StarsCollected.ToString();
    public string Points => (highestYPosition * 100).ToString("F0");
    public string FinalScore => finalScore.ToString();
    
    
    void Start()
    {
        // Get the AdManager component from a GameObject in the scene
        adManager = FindObjectOfType<AdManager>();
        // Load control setting from PlayerPrefs
        int savedControlType = PlayerPrefs.GetInt("ControlType", (int)ControlOptionsManager.ControlType.Touch);
        controlType = (ControlOptionsManager.ControlType)savedControlType;
        ApplyControlType();
        ApplySpriteType();
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        lastGroundedTime = Time.time;
        highestYPosition = transform.position.y; // Initialize highestYPosition with initial player position
        TotalStarsForShop = PlayerPrefs.GetInt("TotalStarsForShop", 0); // Moved from Start()
    }

    void Update()
    {
        float thresholdY = mainCamera.transform.position.y - minYPosition;
        if (transform.position.y < thresholdY)
        {
            finalScore = int.Parse(Points) + (StarsCollected * 3000);
            DisplayGameOverScreen();
            Destroy(gameObject);
            // Check if AdManager is found
            if (adManager != null)
            {
                // Call the ShowInterstitialAd function from the AdManager script
                adManager.ShowInterstitialAd();
            }
            else
            {
                Debug.LogError("AdManager not found!");
            }
        }

        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Handle player movement based on control type
        float moveInput = 0f;

        switch (controlType)
        {
            case ControlOptionsManager.ControlType.Touch:
                moveInput = GetTouchMovement();
                break;

            case ControlOptionsManager.ControlType.Tilt:
                float tiltInput = Input.acceleration.x;
                moveInput = tiltInput * tiltSensitivity;
                break;
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        float GetTouchMovement()
        {
            // Check if there is any touch input
            if (Input.touchCount > 0)
            {
                // Get the first touch
                Touch touch = Input.GetTouch(0);

                // Get the position of the touch
                float touchX = touch.position.x;

                // Get the half width of the screen
                float halfScreenWidth = Screen.width * 0.5f;

                // Check if the touch position is on the left half of the screen
                if (touchX < halfScreenWidth)
                {
                    return -1f; // Move left
                }
                else
                {
                    return 1f; // Move right
                }
            }

            // If no touch input, return 0
            return 0f;
        }


        // Teleportation at screen edges
        TeleportAtScreenEdges();
        if (transform.position.y > highestYPosition)
        {
            highestYPosition = transform.position.y;
        }

        // Jumping
        if (isGrounded)
        {
            if (Time.time - lastGroundedTime >= autoJumpDelay)
            {
                // Play jump sound
                if (jumpSound != null)
                {
                    jumpSound.Play();
                }

                // Play jump particles
                PlayJumpParticles();

                // Apply jump force
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                lastGroundedTime = Time.time;
                isFalling = true; // Set isFalling to true when the player jumps

                // Toggle between RedSquare and BlueSquare
                if (isRedSquareActive)
                {
                    RedSquare.GetComponent<SpriteRenderer>().enabled = true;
                    BlueSquare.GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    RedSquare.GetComponent<SpriteRenderer>().enabled = false;
                    BlueSquare.GetComponent<SpriteRenderer>().enabled = true;
                }

                // Update the state for the next jump
                isRedSquareActive = !isRedSquareActive;
            }
        }

        else
        {
            // Set isFalling to true when the player is not grounded and is moving downward
            isFalling = rb.velocity.y < 0f;
        }
        ExcludeGroundCollisions(!isFalling);
    }
    void ApplySpriteType(){
        int currentSpriteType = PlayerPrefs.GetInt("SpriteType");
        if(currentSpriteType == 1){
            WhiteSquare.GetComponent<SpriteRenderer>().enabled = false;
            GreenSquare.GetComponent<SpriteRenderer>().enabled = false;
            NeonSquare2.GetComponent<SpriteRenderer>().enabled = false;
        }
        if(currentSpriteType == 0){
            WhiteSquare.GetComponent<SpriteRenderer>().enabled = true;
            GreenSquare.GetComponent<SpriteRenderer>().enabled = false;
            NeonSquare2.GetComponent<SpriteRenderer>().enabled = false;
        }
        if(currentSpriteType == 2){
            WhiteSquare.GetComponent<SpriteRenderer>().enabled = true;
            GreenSquare.GetComponent<SpriteRenderer>().enabled = true;
            NeonSquare2.GetComponent<SpriteRenderer>().enabled = false;
        }
        if(currentSpriteType == 3){
            WhiteSquare.GetComponent<SpriteRenderer>().enabled = false;
            GreenSquare.GetComponent<SpriteRenderer>().enabled = false;
            NeonSquare2.GetComponent<SpriteRenderer>().enabled = true;
        }
    } 
    float GetTouchMovement()
    {
        // Check if there is any touch input
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            // Get the position of the touch
            float touchX = touch.position.x;

            // Get the half width of the screen
            float halfScreenWidth = Screen.width * 0.5f;

            // Check if the touch position is on the left half of the screen
            if (touchX < halfScreenWidth)
            {
                return -1f; // Move left
            }
            else
            {
                return 1f; // Move right
            }
        }

        // If no touch input, return 0
        return 0f;
    }

    void FixedUpdate()
    {
        // Visualize ground check position
        Debug.DrawRay(groundCheck.position, Vector2.down * 0.1f, Color.red);
    }
    private void ApplyControlType()
    {
        switch (controlType)
        {
            case ControlOptionsManager.ControlType.Touch:
                // Set up touch controls
                break;

            case ControlOptionsManager.ControlType.Tilt:
                // Set up tilt controls
                break;
        }
    }
    public void SetControlType(int type)
    {
        controlType = (ControlOptionsManager.ControlType)type;
        PlayerPrefs.SetInt("ControlType", (int)controlType);
        PlayerPrefs.Save();
        ApplyControlType();
    }
    



    // Function to exclude collisions with ground
    void ExcludeGroundCollisions(bool exclude)
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, groundLayer, exclude);
    }

    // Function to teleport the player at screen edges
    void TeleportAtScreenEdges()
    {
        // Get the viewport position of the player
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        // Check if the player is at the left edge of the screen
        if (viewportPosition.x <= 0f)
        {
            // Teleport to the right side of the screen
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1f, viewportPosition.y, viewportPosition.z));
        }
        // Check if the player is at the right edge of the screen
        else if (viewportPosition.x >= 1f)
        {
            // Teleport to the left side of the screen
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0f, viewportPosition.y, viewportPosition.z));
        }
    }



    // OnTriggerEnter2D is called when the Collider2D other enters the trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jumper"))
        {
            if (JumperSound != null)
                {
                    JumperSound.Play();
                }
            
            if (!isGrounded) // Only add moreJumpForce if the player is not grounded
            {
                // Add adjustable y-axis velocity to the player
                rb.velocity = new Vector2(rb.velocity.x, jumpForce + moreJumpForce);
            }
            else
            {
                // If grounded, just apply the regular jump force
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
        
        else if (other.CompareTag("Star")) // Check if the collision is with a star and has not already collided
        { 
            StarsCollected++;
            AddStarToShop();
            Destroy(other.gameObject); 
        }
    }
    
    public void AddStarToShop()
    {
        TotalStarsForShop++; // Increment TotalStarsForShop
        PlayerPrefs.SetInt("TotalStarsForShop", TotalStarsForShop); // Save to PlayerPrefs
    }
    public bool IsFalling()
    {
        return isFalling;
    }
    public bool IsGrounded()
    {
        return isGrounded;

    }
    private void PlayJumpParticles()
    {
        // Check if the particle system prefab is assigned
        if (jumpParticlesPrefab != null)
        {
            // Calculate the position to spawn the particles
            Vector3 spawnPosition = transform.position;
            spawnPosition.y -= GetComponent<Collider2D>().bounds.extents.y; // Place the particles at the bottom of the player

            // Instantiate the particle system prefab at the calculated position
            GameObject particles = Instantiate(jumpParticlesPrefab, spawnPosition, Quaternion.identity);

        }



    }
    
    void DisplayGameOverScreen()
{
    gameOverScreenPrefab.SetActive(true);
}

}
