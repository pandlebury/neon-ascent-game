using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text starText;
    public TMP_Text finalScoreText;
    public TMP_Text HighScoreText;
    public TMP_Text totalStarsForShopText; // TextMeshPro component for displaying total stars for shop
    public Canvas HowToPlayCanvas; // HowToPlay canvas reference

    private PlayerController playerController;

    private bool firstTime; // Flag to track if it's the first time the scene is loaded

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogWarning("PlayerController not found at start. Make sure it's present in the scene.");
        }
        else
        {
            Debug.Log("PlayerController found: " + playerController.gameObject.name);
        }

        if (scoreText == null)
        {
            Debug.LogError("Score text reference is not assigned!");
        }

        // Load the firstTime flag from PlayerPrefs
        firstTime = PlayerPrefs.GetInt("FirstTime", 1) == 1;

        // Check if it's the first time the scene is loaded
        if (firstTime)
        {
            // Display the HowToPlay canvas
            HowToPlayCanvas.gameObject.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            // If it's not the first time, hide the HowToPlay canvas
            HowToPlayCanvas.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (firstTime && Input.touchCount > 0) // Check for touch input if it's the first time
        {
            // If there's touch input, hide the HowToPlay canvas and resume the game
            HowToPlayCanvas.gameObject.SetActive(false);
            Time.timeScale = 1f; // Resume the game
            firstTime = false; // Set the flag to false to prevent displaying the canvas again

            // Save the updated firstTime flag to PlayerPrefs
            PlayerPrefs.SetInt("FirstTime", 0);
            PlayerPrefs.Save();
        }

        if (playerController != null)
        {
            starText.text = "Squares: " + playerController.StarsCollected.ToString();
            scoreText.text = "Score: " + playerController.Points.ToString();
            finalScoreText.text = "Final Score: " + playerController.finalScore.ToString("N0");
            DisplayHighScore();
            CheckHighScore();
            // Update Total Stars for Shop text
            totalStarsForShopText.text = "Total Squares" + playerController.TotalStarsForShop.ToString();
        }
        else
        {
            Debug.LogWarning("PlayerController not assigned in LogicScript.");
        }
    }

    void DisplayHighScore()
    {
        float highScore = PlayerPrefs.GetFloat("HighScore", 0);
        HighScoreText.text = "HIGHSCORE " + highScore.ToString("N0");
    }

    void CheckHighScore()
    {
        float highScore = PlayerPrefs.GetFloat("HighScore", 0);
        if (playerController.finalScore > highScore)
        {
            PlayerPrefs.SetFloat("HighScore", playerController.finalScore);
            PlayerPrefs.Save();
            DisplayHighScore();
        }
    }

    

    

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
