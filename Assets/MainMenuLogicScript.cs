using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuLogicScript : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;

    public Button optionsButton;
    public Button ShopButton;


    void Start()
    {
        // Add listeners to the buttons
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
        optionsButton.onClick.AddListener(Options);
        ShopButton.onClick.AddListener(Shop);

    }

    void StartGame()
    {
        // Load the game scene (replace "GameScene" with the name of your actual game scene)
        SceneManager.LoadScene("SampleScene");
    }
    void Options(){
        
        SceneManager.LoadScene("OptionsScene");

    }
    void Shop(){
        SceneManager.LoadScene("ShopScene");
    }

    void QuitGame()
    {
        // Quit the application
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
