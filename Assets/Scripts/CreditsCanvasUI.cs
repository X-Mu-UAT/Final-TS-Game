using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Manages the rolling display of game contributors and menu navigation routing.
/// </summary>
public class CreditsCanvasUI : MonoBehaviour
{
    [Header("UI Component Bindings")]
    [SerializeField] private TMP_Text creditsDisplayTextBox;
    [SerializeField] private Button backToMainMenuButton;

    [Header("Credits Data List (Designer Customizable)")]
    [SerializeField]
    private List<string> projectContributorsList = new List<string>()
    {
        "=== FROG PLATFORMER CREDITS ===",
        "",
        "--- DESIGN & DEVELOPMENT ---",
        "Lead Frog Architect - [Muddaththir Waheed-Hill]",
        "",
        "--- ART & SPRITES ---",
        "Environmental Asset Designer - OpenGameArt",
        "Frog Animation Suite - Creative Commons Asset Hub",
        "",
        "--- SOUND & MUSIC ---",
        "Mixer Routing & SFX Filters - AudioManager Pipeline",
        "",
        "Thank you for playing my project build!"
    };

    [Header("Rolling Settings (Framerate Independent)")]
    [SerializeField] private float textRollSpeedMultiplier = 40f;
    [SerializeField] private float textResetYPositionThreshold = 800f;

    private Vector3 initialTextStartingPosition;

    private void Start()
    {
        // Programmatically assign button routing listeners cleanly
        if (backToMainMenuButton != null)
        {
            backToMainMenuButton.onClick.AddListener(ReturnToMainMenuContext);
        }

        // Cache text positions on startup to handle movement oscillation loops smoothly
        if (creditsDisplayTextBox != null)
        {
            initialTextStartingPosition = creditsDisplayTextBox.transform.position;
        }

        AssembleAndFormatCreditsText();
    }

    private void Update()
    {
        // UAT REQUIREMENT: Move an object by explicitly setting its transform position coordinates directly
        // The movement updates are framerate independent using Time.deltaTime multiplication arrays
        if (creditsDisplayTextBox != null)
        {
            creditsDisplayTextBox.transform.position += Vector3.up * (textRollSpeedMultiplier * Time.deltaTime);

            // Oscillate or restart the scrolling credits loop if it moves past the cutoff box limits
            if (creditsDisplayTextBox.transform.localPosition.y > textResetYPositionThreshold)
            {
                creditsDisplayTextBox.transform.position = initialTextStartingPosition;
            }
        }
    }

    private void AssembleAndFormatCreditsText()
    {
        if (creditsDisplayTextBox == null) return;

        string finalizedRollingCreditsString = "";

        // UAT REQUIREMENT: Demonstrate use of at least one foreach loop structure container 
        foreach (string attributionLine in projectContributorsList)
        {
            finalizedRollingCreditsString += attributionLine + "\n";
        }

        // Assign the string block straight to your stretching text box component
        creditsDisplayTextBox.text = finalizedRollingCreditsString;
    }

    private void ReturnToMainMenuContext()
    {
        // Return smoothly using your singular central instance structure
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReturnToMainMenu();
        }
        else
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
