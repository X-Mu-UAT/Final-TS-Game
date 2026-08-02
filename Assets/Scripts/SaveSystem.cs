using System.IO;
using UnityEngine;

/// <summary>
/// Static management pipeline writing structure configs to the user's persistent system directories.
/// </summary>
public static class SaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "game_session_profile.json");

    public static void SaveGame(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    public static GameData LoadGame()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        return new GameData();
    }
}
