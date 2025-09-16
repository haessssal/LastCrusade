using UnityEngine;

public class MainManager : MonoBehaviour
{
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    void Start()
    {
        SpawnCharacters();
    }

    private void SpawnCharacters()
    {
        // P1 spawn
        CharacterData p1Data = GameManager.instance.player1Data;
        if (p1Data != null)
        {
            GameObject player1Object = Instantiate(p1Data.characterPrefab, player1SpawnPoint.position, Quaternion.identity);
            HeroKnight player1Script = player1Object.GetComponent<HeroKnight>();
            if (player1Script != null) player1Script.SetCharacterStats(p1Data);
        }

        // P2 spawn
        CharacterData p2Data = GameManager.instance.player2Data;
        if (p2Data != null)
        {
            GameObject player2Object = Instantiate(p2Data.characterPrefab, player2SpawnPoint.position, Quaternion.identity);
            HeroKnight player2Script = player2Object.GetComponent<HeroKnight>();
            if (player2Script != null) player2Script.SetCharacterStats(p2Data);
        }
    }
}