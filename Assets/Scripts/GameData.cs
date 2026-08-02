using System;

/// <summary>
/// Data container for session persistence using JSON Serialization.
/// </summary>
[Serializable]
public class GameData
{
    public int score;
    public float musicVolume;
    public float sfxVolume;

    // Sets default initialization variables if configuration file does not exist.
    public GameData()
    {
        score = 0;
        musicVolume = 0.75f;
        sfxVolume = 0.75f;
    }
}
