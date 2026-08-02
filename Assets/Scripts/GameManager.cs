using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Core structural system managing game states, score metrics, and persistence pipelines.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton Instance
    public static GameManager Instance { get; private set; }

    [Header("Session Progress Storage")]
    [SerializeField] private int currentScore = 0;

    // Abstract collection tracking loaded game levels dynamically via string indices
    private Dictionary<string, int> sceneBuildIndexes = new Dictionary<string, int>();
    private List<string> activeGameModifiers = new List<string>();

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
        DontDestroyOnLoad(gameObject);

        LoadProfileData();
        PopulateSceneDictionary();
    }

    private void PopulateSceneDictionary()
    {
        // Demonstrating a for loop implementation to track operational scenes
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            // Note: Since real names can't always be parsed from closed settings dynamically without editor utilities,
            // we simulate index records within our runtime structure container mapping keys safely.
            sceneBuildIndexes.Add("Level_" + i, i);
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        if (currentScore > SessionData.score)
        {
            SessionData.score = currentScore;
            SaveProfileData();
        }
    }

    public int GetCurrentScore() => currentScore;

    public void LoadProfileData()
    {
        SessionData = SaveSystem.LoadGame();
    }

    public void SaveProfileData()
    {
        SaveSystem.SaveGame(SessionData);
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
