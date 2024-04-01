using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour
{
    [SerializeField] Image soundOnIcon;
    [SerializeField] Image soundOffIcon;
    private bool muted = false;
    // Start is called before the first frame update
    void Start()
    {
    if (!PlayerPrefs.HasKey("muted"))
    {
        // Set the default muted state to false
        PlayerPrefs.SetInt("muted", 0);
    }

    Load(); // Load the muted state from PlayerPrefs

    // Unmute the audio regardless of the loaded state
    AudioListener.pause = false;  // Ensure sound is enabled initially

    UpdateButtonIcon(); // Update button icon based on the loaded muted state
    }
    public void OnButtonPress()
    {
        if (muted == false)
        {
            muted = true;
            AudioListener.pause = true;

        }
        else
        {
            muted = false;
            AudioListener.pause = false;

        }
        Save();
        UpdateButtonIcon();
    }
    private void UpdateButtonIcon()
    {
        if(muted == false)
        {
            soundOnIcon.enabled = true;
            soundOffIcon.enabled = false;
        }
        else
        {
            soundOnIcon.enabled = false;
            soundOffIcon.enabled = true;
        }
    }
    private void Load()
    {
        muted = PlayerPrefs.GetInt("muted") == 1;
    }
    private void Save()
    {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }
}