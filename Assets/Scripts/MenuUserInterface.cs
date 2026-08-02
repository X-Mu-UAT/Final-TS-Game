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
    [SerializeField] private Button actionExitButton;
    [SerializeField] private Slider backgroundMusicSlider;
    [SerializeField] private Slider environmentalSFXSlider;
    [SerializeField] private TMP_Text highscoreRecordTextBox;

    private bool hasGameSessionBegun = false;

    private void Start()
    {
        // Enforce structural window layout settings explicitly upon launch
        initialSplashPanel.SetActive(true);
        menuNavigationPanel.SetActive(false);
        structuralSettingsPanel.SetActive(false);

        // Bind interactive UI listeners programmatically
        actionStartButton.onClick.AddListener(StartGameplaySession);
        actionSettingsButton.onClick.AddListener(OpenSettingsMenu);
        actionExitButton.onClick.AddListener(QuitApplicationContext);

        backgroundMusicSlider.onValueChanged.AddListener(UpdateMusicVolumeSettings);
        environmentalSFXSlider.onValueChanged.AddListener(UpdateSFXVolumeSettings);

        DisplayPersistedHighscores();
    }

    private void Update()
    {
        if (!hasGameSessionBegun && Input.anyKeyDown)
        {
            hasGameSessionBegun = true;
            initialSplashPanel.SetActive(false);
            menuNavigationPanel.SetActive(true);
        }
    }

    private void StartGameplaySession()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    private void OpenSettingsMenu()
    {
        structuralSettingsPanel.SetActive(!structuralSettingsPanel.activeSelf);
    }

    private void UpdateMusicVolumeSettings(float value)
    {
        AudioManager.Instance.ModifyMusicChannelVolume(value);
    }

    private void UpdateSFXVolumeSettings(float value)
    {
        AudioManager.Instance.ModifySFXChannelVolume(value);
    }

    private void DisplayPersistedHighscores()
    {
        int scoreValue = GameManager.Instance.SessionData.score;
        highscoreRecordTextBox.text = "HISTORIC HIGHSCORE: " + scoreValue.ToString();
    }

    private void QuitApplicationContext()
    {
        GameManager.Instance.QuitGame();
    }
}
