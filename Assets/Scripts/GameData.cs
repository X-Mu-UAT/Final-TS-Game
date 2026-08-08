using System;
using System.Collections.Generic;

[Serializable]
public class HighscoreEntry
{
    public string playerName;
    public int scoreValue;

    public HighscoreEntry(string name, int score)
    {
        playerName = name;
        scoreValue = score;
    }
}

[Serializable]
public class GameData
{
    // Tracks a collection of historical score entries
    public List<HighscoreEntry> scoresHistoryList = new List<HighscoreEntry>();
    public float musicVolume;
    public float sfxVolume;

    public GameData()
    {
        musicVolume = 0.75f;
        sfxVolume = 0.75f;

        // Populate with structural default entries for the leaderboard placeholder display
        scoresHistoryList.Add(new HighscoreEntry("🐸 Froggy", 1200));
        scoresHistoryList.Add(new HighscoreEntry("🪰 LilyPad", 800));
        scoresHistoryList.Add(new HighscoreEntry("🪨 Tadpole", 300));
    }
}
