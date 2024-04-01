using UnityEngine;
using TMPro;
using System.Collections;

public class SmoothColorChanger : MonoBehaviour
{
    public float colorChangeInterval = 1.0f; // Interval between color changes
    public float transitionDuration = 1.0f; // Duration of color transition
    private TextMeshProUGUI textMeshPro;
    private Color targetColor;
    private Color initialColor;

    void Start()
    {
        // Get the TextMeshPro component attached to the game object
        textMeshPro = GetComponent<TextMeshProUGUI>();


        // Set initial color
        initialColor = textMeshPro.color;

        // Start color change coroutine
        StartCoroutine(ChangeColorSmoothly());
    }

    IEnumerator ChangeColorSmoothly()
    {
        while (true)
        {
            // Set target color to a random color
            targetColor = Random.ColorHSV();

            // Interpolate color gradually over transition duration
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / transitionDuration;
                textMeshPro.color = Color.Lerp(initialColor, targetColor, t);
                yield return null;
            }

            // Set initial color for next transition
            initialColor = targetColor;

            // Wait for the specified interval before starting the next transition
            yield return new WaitForSeconds(colorChangeInterval);
        }
    }
}
