using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BackgroundSound : MonoBehaviour
{
    private static BackgroundSound backgroundMusic;
    private AudioSource audioSource;

    // Add your background music clip to this field in the Unity Editor
    public AudioClip backgroundMusicClip;

    // Start is called before the first frame update
    void Awake()
    {
        if (backgroundMusic == null)
        {
            backgroundMusic = this;
            DontDestroyOnLoad(backgroundMusic.gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add an AudioSource component if not already attached
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the background music clip
        audioSource.clip = backgroundMusicClip;

        // Make the music loop
        audioSource.loop = true;

        // Play the background music
        audioSource.Play();
    }
}
