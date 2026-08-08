using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Core UI Controller managing Screen-Space Canvas interactions.
/// </summary>
public class MenuUserInterface : MonoBehaviour
{
    [Header("Interface Elements")]
    [SerializeField] private GameObject initialSplashPanel;
    [SerializeField] private GameObject menuNavigationPanel;
    [SerializeField] private GameObject structuralSettingsPanel;

    [Header("Interactive Input Selectors")]
    [SerializeField] private Button actionStartButton;
    [SerializeField] private Button actionSettingsButton;
    [SerializeField] private Button actionCloseSettingsButton; // Back button variable reference
    [SerializeField] private Button actionExitButton;
    [SerializeField] private Slider backgroundMusicSlider;
    [SerializeField] private Slider environmentalSFXSlider;
    [SerializeField] private TMP_Text highscoreRecordTextBox;

    private bool hasGameSessionBegun = false;

    private void Start()
    {
        // Enforce structural window layout settings explicitly upon launch (guard nulls)
        if (initialSplashPanel != null) initialSplashPanel.SetActive(true);
        if (menuNavigationPanel != null) menuNavigationPanel.SetActive(false);
        if (structuralSettingsPanel != null) structuralSettingsPanel.SetActive(false);

        // Bind interactive UI listeners programmatically (guard assignments)
        if (actionStartButton != null) actionStartButton.onClick.AddListener(StartGameplaySession);
        if (actionSettingsButton != null) actionSettingsButton.onClick.AddListener(OpenSettingsMenu);
        if (actionExitButton != null) actionExitButton.onClick.AddListener(QuitGame);

        if (actionCloseSettingsButton != null)
        {
            actionCloseSettingsButton.onClick.AddListener(CloseSettingsMenu);
        }

        if (backgroundMusicSlider != null) backgroundMusicSlider.onValueChanged.AddListener(UpdateMusicVolumeSettings);
        if (environmentalSFXSlider != null) environmentalSFXSlider.onValueChanged.AddListener(UpdateSFXVolumeSettings);

        DisplayPersistedHighscores();
    }

    private void Update()
    {
        if (!hasGameSessionBegun)
        {
            bool inputDetected = false;
            try
            {
                inputDetected = Input.anyKeyDown;
            }
            catch (System.InvalidOperationException)
            {
                // New Input System active - attempt reflective query to avoid compile-time dependency
                var keyboardType = System.Type.GetType("UnityEngine.InputSystem.Keyboard, Unity.InputSystem");
                if (keyboardType != null)
                {
                    var currentProp = keyboardType.GetProperty("current");
                    if (currentProp != null)
                    {
                        var keyboardInstance = currentProp.GetValue(null, null);
                        if (keyboardInstance != null)
                        {
                            var anyKeyProp = keyboardType.GetProperty("anyKey");
                            if (anyKeyProp != null)
                            {
                                var anyKeyObj = anyKeyProp.GetValue(keyboardInstance, null);
                                if (anyKeyObj != null)
                                {
                                    var wasPressedProp = anyKeyObj.GetType().GetProperty("wasPressedThisFrame");
                                    if (wasPressedProp != null)
                                    {
                                        var val = wasPressedProp.GetValue(anyKeyObj, null);
                                        if (val is bool b && b) inputDetected = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (inputDetected)
            {
                hasGameSessionBegun = true;
                if (initialSplashPanel != null) initialSplashPanel.SetActive(false);
                if (menuNavigationPanel != null) menuNavigationPanel.SetActive(true);
            }
        }
    }

    private void StartGameplaySession()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level 1-2");
    }

    private void OpenSettingsMenu()
    {
        structuralSettingsPanel.SetActive(true);
    }

    private void CloseSettingsMenu()
    {
        structuralSettingsPanel.SetActive(false);
    }

    private void UpdateMusicVolumeSettings(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ModifyMusicChannelVolume(value);
        }
        else
        {
            Debug.LogWarning("AudioManager.Instance is null - cannot update music volume.");
        }
    }

    private void UpdateSFXVolumeSettings(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ModifySFXChannelVolume(value);
        }
        else
        {
            Debug.LogWarning("AudioManager.Instance is null - cannot update SFX volume.");
        }
    }

    private void DisplayPersistedHighscores()
    {
        if (highscoreRecordTextBox == null)
        {
            Debug.LogWarning("Highscore text box is not assigned in the inspector.");
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.SessionData != null)
        {
            int highestScoreValue = 0;

            // FIXED FOR MULTI-SCORE LIST: Target index [0] to read the highest value in the score container safely
            if (GameManager.Instance.SessionData.scoresHistoryList != null && GameManager.Instance.SessionData.scoresHistoryList.Count > 0)
            {
                highestScoreValue = GameManager.Instance.SessionData.scoresHistoryList[0].scoreValue;
            }

            highscoreRecordTextBox.text = "HISTORIC HIGHSCORE: " + highestScoreValue.ToString();
        }
        else
        {
            highscoreRecordTextBox.text = "HISTORIC HIGHSCORE: 0";
        }
    }

    public void QuitGame()
    {
        if (GameManager.Instance != null)
        {
            // Persist changes and shut down using our GameManager singleton configuration
            GameManager.Instance.QuitGame();
        }
        else
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }
    }
}
