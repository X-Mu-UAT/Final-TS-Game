using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls UI bindings on the Game Over Screen canvas, allowing transitions back to active gameplay.
/// </summary>
public class GameOverMenuManager : MonoBehaviour
{
    [Header("Interactive Button Routing")]
    [SerializeField] private Button retryGameplayButton;
    [SerializeField] private Button viewLeaderboardButton; // ADDED: Highscore scene navigation button
    [SerializeField] private Button exitToMainMenuButton;

    [Header("Audio Configurations (UAT Requirement)")]
    [SerializeField] private AudioSource interfaceAudioSource;
    [SerializeField] private AudioClip interactiveClickSound;

    private void Start()
    {
        // Programmatically assign event listeners to ensure UI actions fire correctly
        if (retryGameplayButton != null)
        {
            retryGameplayButton.onClick.AddListener(TriggerGameplayReset);
        }

        if (viewLeaderboardButton != null)
        {
            viewLeaderboardButton.onClick.AddListener(RouteToHighscoreLeaderboard);
        }

        if (exitToMainMenuButton != null)
        {
            exitToMainMenuButton.onClick.AddListener(RouteBackToMainMenu);
        }
    }

    private void TriggerGameplayReset()
    {
        PlayFeedbackSound();

        // Reload the initial level scene configuration directly 
        SceneManager.LoadScene("Level1");
    }

    private void RouteToHighscoreLeaderboard()
    {
        PlayFeedbackSound();

        // FIXED: Transition the scene window context over to your Highscore leaderboard view 
        SceneManager.LoadScene("HighscoreScene");
    }

    private void RouteBackToMainMenu()
    {
        PlayFeedbackSound();

        // Use the centralized instance manager to reset internal score values safely
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReturnToMainMenu();
        }
        else
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    private void PlayFeedbackSound()
    {
        // UAT REQUIREMENT: Uses AudioSource.PlayOneShot to sound clip feedback effects
        if (interfaceAudioSource != null && interactiveClickSound != null)
        {
            interfaceAudioSource.PlayOneShot(interactiveClickSound);
        }
    }
}
