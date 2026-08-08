using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Fetches structural score tracking histories and dynamically formats them into a leaderboard layout.
/// </summary>
public class HighscoreLeaderboardUI : MonoBehaviour
{
    [Header("UI Component Bindings")]
    [SerializeField] private TMP_Text leaderboardTextBox;
    [SerializeField] private Button backToMenuButton;

    // UAT REQUIREMENT: Use at least one Dictionary container mapping data states cleanly
    private Dictionary<string, int> mappedLeaderboardData = new Dictionary<string, int>();

    private void Start()
    {
        if (backToMenuButton != null)
        {
            backToMenuButton.onClick.AddListener(LoadMainMenuContextScene);
        }

        PopulateAndRenderLeaderboard();
    }

    private void PopulateAndRenderLeaderboard()
    {
        if (GameManager.Instance == null || GameManager.Instance.SessionData == null || leaderboardTextBox == null) return;

        mappedLeaderboardData.Clear();
        int itemIndex = 1;

        // Pull saved data fields out from our GameManager list and inject them into our local dictionary tracking layout
        foreach (HighscoreEntry entry in GameManager.Instance.SessionData.scoresHistoryList)
        {
            // Create a unique key formatting name row records safely (e.g. "1. Froggy")
            string registryKeyName = $"{itemIndex}. {entry.playerName}";

            if (!mappedLeaderboardData.ContainsKey(registryKeyName))
            {
                mappedLeaderboardData.Add(registryKeyName, entry.scoreValue);
                itemIndex++;
            }
        }

        // Prepare the visual rendering string layout
        string structuralTextOutput = "=== ALL-TIME LEADERBOARD ===\n\n";

        // UAT REQUIREMENT: Use a foreach loop to parse a Dictionary structural configuration matrix
        foreach (KeyValuePair<string, int> scoreboardRecord in mappedLeaderboardData)
        {
            // Build out formatted row blocks for the stretching UI canvas frame draws
            structuralTextOutput += $"{scoreboardRecord.Key} ............ {scoreboardRecord.Value} PTS\n";
        }

        // Push the final formatted string calculation text directly onto your TextMeshPro component
        leaderboardTextBox.text = structuralTextOutput;
    }

    private void LoadMainMenuContextScene()
    {
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
