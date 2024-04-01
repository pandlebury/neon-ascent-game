using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Toggle controlToggle; // Renamed from touchToggle for clarity

    

    void Start()
    {
        int savedControlType = PlayerPrefs.GetInt("ControlType", (int)ControlOptionsManager.ControlType.Touch);
        controlToggle.isOn = savedControlType == (int)ControlOptionsManager.ControlType.Tilt; // Set toggle based on saved type
    }

    public void OnToggleValueChanged(bool isOn)
    {
        ControlOptionsManager.SetControlType(isOn ? (int)ControlOptionsManager.ControlType.Tilt : (int)ControlOptionsManager.ControlType.Touch);
        PlayerPrefs.SetInt("ControlType", isOn ? 1 : 0); // Save 1 for Tilt, 0 for Touch
        PlayerPrefs.Save();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
