using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Core structural system managing game states, score metrics, and persistence pipelines.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton Instance property
    public static GameManager Instance { get; private set; }

    [Header("Session Tracking")]
    [SerializeField] private int currentScore = 0;

    // Persistent profile data container
    public GameData SessionData { get; private set; }

    private void Awake()
    {
        // Enforce the Singleton structural layout configuration
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persists across level switches

        LoadProfileData();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        Debug.Log($"Score incremented by {amount}. New current score: {currentScore}");
    }

    public int GetCurrentScore() => currentScore;

    public int GetHighestCalculatedScore()
    {
        // SAFELY LOOKS UP THE TOP SCORE VALUE: Checks index 0 of your scoreboard history list
        // FIXED SYNTAX: Explicitly checks index [0] to extract the scoreValue parameter property out of the HighscoreEntry safely
        if (SessionData != null && SessionData.scoresHistoryList != null && SessionData.scoresHistoryList.Count > 0)
        {
            return SessionData.scoresHistoryList[0].scoreValue;
        }
        return 0;
    }

    /// <summary>
    /// INJECTS NEW SCORE ENTRIES: Adds a named record directly to our persistent profile list.
    /// </summary>
    public void RecordAndSaveCurrentScore(string nameToLog)
    {
        if (SessionData == null || SessionData.scoresHistoryList == null) return;

        // Add the fresh record to the structural collection profile list
        SessionData.scoresHistoryList.Add(new HighscoreEntry(nameToLog, currentScore));

        // Sort the list from highest score to lowest score using an inline sorting calculation
        SessionData.scoresHistoryList.Sort((entry1, entry2) => entry2.scoreValue.CompareTo(entry1.scoreValue));

        // Enforce a maximum cutoff limit of the top 5 records to save space
        if (SessionData.scoresHistoryList.Count > 5)
        {
            SessionData.scoresHistoryList.RemoveRange(5, SessionData.scoresHistoryList.Count - 5);
        }

        SaveProfileData();
    }

    public void LoadProfileData()
    {
        SessionData = SaveSystem.LoadGame();
    }

    public void SaveProfileData()
    {
        if (SessionData != null)
        {
            SaveSystem.SaveGame(SessionData);
        }
    }

    public void TriggerGameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void ReturnToMainMenu()
    {
        currentScore = 0;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void QuitGame()
    {
        SaveProfileData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
