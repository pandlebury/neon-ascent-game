using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public TMP_Text totalStarsText;
    public Button[] buttons; // Array to hold multiple buttons
    private const string SELECT_TEXT = "SELECT";
    private const int FIRST_ITEM_COST = 25;
    private const int SECOND_ITEM_COST = 0;
    private const int THIRD_ITEM_COST = 35;
    private const int FOURTH_ITEM_COST = 50;

    void Start()
    {
        UpdateTotalStarsForShopText();

        

        UpdateButtonStates();
    }
    void Update()
    {
        UpdateTotalStarsForShopText(); // Continuously update total stars text
    }

    void UpdateButtonStates()
    {
        ChangeTextOfButton(0, PlayerPrefs.GetInt("FirstBuyed") == 1 ? SELECT_TEXT : $"BUY ({FIRST_ITEM_COST} stars)");
        ChangeTextOfButton(1, PlayerPrefs.GetInt("SecondBuyed") == 1 ? SELECT_TEXT : $"BUY ({SECOND_ITEM_COST} stars)");
        ChangeTextOfButton(2, PlayerPrefs.GetInt("ThirdBuyed") == 1 ? SELECT_TEXT : $"BUY ({THIRD_ITEM_COST} stars)");
        ChangeTextOfButton(3, PlayerPrefs.GetInt("FourthBuyed") == 1 ? SELECT_TEXT : $"BUY ({FOURTH_ITEM_COST} stars)");
    }

    void UpdateTotalStarsForShopText()
    {
        totalStarsText.text = "Squares: " + PlayerPrefs.GetInt("TotalStarsForShop").ToString();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void BuyItem(int itemIndex, int itemCost)
    {
        int currentStars = PlayerPrefs.GetInt("TotalStarsForShop");

        if (currentStars >= itemCost)
        {
            PlayerPrefs.SetInt("TotalStarsForShop", currentStars - itemCost);
            PlayerPrefs.SetInt($"SpriteType", itemIndex);
            PlayerPrefs.SetInt($"{GetItemName(itemIndex)}Buyed", 1);
            UpdateButtonStates();
        }
        else
        {
            Debug.LogWarning("Not enough stars to purchase the item.");
        }
    }

    public void FirstItemButton()
    {
        if (PlayerPrefs.GetInt("FirstBuyed") == 1)
        {
            PlayerPrefs.SetInt("SpriteType", 1);
        }
        else
        {
            BuyItem(0, FIRST_ITEM_COST);
        }
    }

    public void SecondItemButton()
    {
        if (PlayerPrefs.GetInt("SecondBuyed") == 1)
        {
            PlayerPrefs.SetInt("SpriteType", 0);
        }
        else
        {
            BuyItem(1, SECOND_ITEM_COST);
        }
    }

    public void ThirdItemButton()
    {
        if (PlayerPrefs.GetInt("ThirdBuyed") == 1)
        {
            PlayerPrefs.SetInt("SpriteType", 2);
        }
        else
        {
            BuyItem(2, THIRD_ITEM_COST);
        }
    }

    public void FourthItemButton()
    {
        if (PlayerPrefs.GetInt("FourthBuyed") == 1)
        {
            PlayerPrefs.SetInt("SpriteType", 3);
        }
        else
        {
            BuyItem(3, FOURTH_ITEM_COST);
        }
    }

    // Function to change the text of a button by index
    public void ChangeButtonLabel(int buttonIndex, string newText)
    {
        // Check if the index is valid
        if (buttonIndex >= 0 && buttonIndex < buttons.Length)
        {
            // Access the TextMeshProUGUI component of the button
            TextMeshProUGUI buttonText = buttons[buttonIndex].GetComponentInChildren<TextMeshProUGUI>();

            // Check if the TextMeshProUGUI component exists
            if (buttonText != null)
            {
                // Change the text of the button
                buttonText.text = newText;
            }
            else
            {
                Debug.LogError("TextMeshProUGUI component not found in Button " + buttonIndex);
            }
        }
        else
        {
            Debug.LogError("Invalid button index.");
        }
    }

    // Example of changing the text of a specific button
    public void ChangeTextOfButton(int buttonIndex, string newText)
    {
        ChangeButtonLabel(buttonIndex, newText);
    }

    private string GetItemName(int itemIndex)
    {
        return itemIndex switch
        {
            0 => "First",
            1 => "Second",
            2 => "Third",
            3 => "Fourth",
            _ => ""
        };
    }
}
