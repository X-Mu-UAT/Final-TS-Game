using UnityEngine;
using TMPro; // Required for handling TextMeshPro fields

/// <summary>
/// Monitors and renders the current game score onto the Screen-Space UI Canvas.
/// </summary>
public class UIScoreTracker : MonoBehaviour
{
    [Header("UI Component Bindings")]
    [SerializeField] private TMP_Text scoreDisplayTextBox;
    [SerializeField] private string textPrefixLabel = "SCORE: ";

    private void Start()
    {
        // Automated fallback: If you forget to link it in the inspector, grab it from this object
        if (scoreDisplayTextBox == null)
        {
            scoreDisplayTextBox = GetComponent<TMP_Text>();
        }
    }

    private void Update()
    {
        // Query the core GameManager instance safely to fetch absolute real-time session values
        if (GameManager.Instance != null && scoreDisplayTextBox != null)
        {
            int currentGlobalScore = GameManager.Instance.GetCurrentScore();

            // Render the synchronized text string layout directly onto the canvas text component
            scoreDisplayTextBox.text = textPrefixLabel + currentGlobalScore.ToString();
        }
        else
        {
            // Debug alert to help you verify structural object links in your scene layout
            if (GameManager.Instance == null)
            {
                Debug.LogWarning("UIScoreTracker is active, but an active '_GameManager' object cannot be found in your open level hierarchy!");
            }
        }
    }
}
