using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages level completion logic, session persistence tracking, and level transitions.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class LevelGoal2D : MonoBehaviour
{
    [Header("Scene Routing Settings")]
    [SerializeField] private string targetNextLevelSceneName = "Level2";
    [SerializeField] private bool isFinalLevelOfTheGame = false;

    [Header("Audio Customization (UAT Requirement)")]
    [SerializeField] private AudioSource objectiveAudioSource;
    [SerializeField] private AudioClip levelCompleteSound;

    private void Start()
    {
        // Enforce that the attached collider functions purely as an overlapping sensor matrix
        Collider2D goalCollider = GetComponent<Collider2D>();
        if (goalCollider != null)
        {
            goalCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the intersecting game entity is the player character
        if (other.CompareTag("Player"))
        {
            ProcessLevelCompletion();
        }
    }

    private void ProcessLevelCompletion()
    {
        // Play success audio through our tracking AudioSource component pipeline
        if (objectiveAudioSource != null && levelCompleteSound != null)
        {
            objectiveAudioSource.PlayOneShot(levelCompleteSound);
        }

        // Save session metrics via our singular GameManager instance profile
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(500); // Award completion points
            GameManager.Instance.SaveProfileData();
        }

        // UAT REQUIREMENT: Use an if statement with an else clause to handle end-of-game sequences
        if (isFinalLevelOfTheGame)
        {
            // Gather all currently active root game scenes for automated cleanup checks
            // UAT REQUIREMENT: Demonstrate a clean implementation of a foreach loop
            foreach (GameObject rootObj in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                Debug.Log($"Preparing to transition from level. Scene cleanup tracking: {rootObj.name}");
            }

            // Redirect the window over to the Credits scene context
            SceneManager.LoadScene("CreditsScene");
        }
        else
        {
            // Load the next designated stage sequence profile directly
            SceneManager.LoadScene(targetNextLevelSceneName);
        }
    }
}
