using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WorldData
{
    public int p1Wins = 0;
    public int p2Wins = 0;
    public int matchCount = 0;
    public List<string> matchResults = new List<string>();
}
