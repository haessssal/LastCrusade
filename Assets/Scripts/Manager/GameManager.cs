using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharacterData player1Data;
    public CharacterData player2Data;

    public int p1Wins = 0;
    public int p2Wins = 0;
    public int matchCount = 0;
    public List<string> matchResults = new List<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else Destroy(gameObject);
    }
}
