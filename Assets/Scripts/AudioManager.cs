using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Audio management architecture interacting with AudioMixer channels.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer Allocations")]
    [SerializeField] private AudioMixer targetGlobalAudioMixer;
    [SerializeField] private AudioSource interfaceAudioSource;

    [Header("Exposed Parameter Strings")]
    [SerializeField] private string musicVolumeParameterName = "MusicVol";
    [SerializeField] private string sfxVolumeParameterName = "SFXVol";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ModifyMusicChannelVolume(float volumeValue)
    {
        // Standard mapping configurations converting safe values into logarithmic decibel formats
        float convertedDecibelValue = Mathf.Log10(Mathf.Clamp(volumeValue, 0.0001f, 1f)) * 20f;
        targetGlobalAudioMixer.SetFloat(musicVolumeParameterName, convertedDecibelValue);
    }

    public void ModifySFXChannelVolume(float volumeValue)
    {
        float convertedDecibelValue = Mathf.Log10(Mathf.Clamp(volumeValue, 0.0001f, 1f)) * 20f;
        targetGlobalAudioMixer.SetFloat(sfxVolumeParameterName, convertedDecibelValue);
    }

    public void PlayInterfaceSound(AudioClip clip)
    {
        if (clip != null && interfaceAudioSource != null)
        {
            interfaceAudioSource.PlayOneShot(clip);
        }
    }
}
