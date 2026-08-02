using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates World-Space Canvases dynamically to mirror the status metrics of the player's health.
/// </summary>
public class WorldSpaceHealthBar : MonoBehaviour
{
    [Header("Monitored Player Context")]
    [SerializeField] private Health targetHealthComponent;

    [Header("Target Visual Indicators")]
    [SerializeField] private Slider structuralHealthSliderVisual;

    private void Start()
    {
        if (structuralHealthSliderVisual != null && targetHealthComponent != null)
        {
            // Dynamically synchronize the UI slider max limits with your script metrics on startup
            structuralHealthSliderVisual.maxValue = 5;
        }
    }

    private void Update()
    {
        // Continuously pull data from the Health script property every single frame draw
        if (targetHealthComponent != null && structuralHealthSliderVisual != null)
        {
            structuralHealthSliderVisual.value = targetHealthComponent.CurrentHealth;
        }
    }
}
