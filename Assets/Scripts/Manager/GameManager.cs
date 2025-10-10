using UnityEngine;
using System.Collections.Generic;

public enum WorldType
{
    Ceiling,
    Lightning,
    Gravity,
    Ground
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharacterData player1Data;
    public CharacterData player2Data;
    public WorldType SelectedWorld { get; private set; }
    public Dictionary<WorldType, WorldData> worldRecords = new Dictionary<WorldType, WorldData>();

    // public int p1Wins = 0;
    // public int p2Wins = 0;
    // public int matchCount = 0;
    // public List<string> matchResults = new List<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else Destroy(gameObject);
    }

    public void SetWorld(WorldType world)
    {
        SelectedWorld = world;
    }

    public WorldData GetCurrentWorldData()
    {
        WorldData data;
        if (worldRecords.TryGetValue(SelectedWorld, out data)) return data;

        data = new WorldData();
        worldRecords.Add(SelectedWorld, data);
        return data;
    }

}
