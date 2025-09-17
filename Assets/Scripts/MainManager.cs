using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    public TextMeshProUGUI timerText;
    // P1 UI
    public TextMeshProUGUI p1NameText;
    public TextMeshProUGUI p1HpText;
    public TextMeshProUGUI p1MpText;
    // P2 UI
    public TextMeshProUGUI p2NameText;
    public TextMeshProUGUI p2HpText;
    public TextMeshProUGUI p2MpText;    

    private float matchTime = 120f;
    private bool isMatchOver = false;

    private void Start()
    {
        SpawnCharacters();
        SetPlayerUI();
    }

    private void Update()
    {
        if (!isMatchOver)
        {
            matchTime -= Time.deltaTime;
            timerText.text = Mathf.Max(0, Mathf.FloorToInt(matchTime)).ToString();

            if (matchTime <= 0)
            {
                isMatchOver = true;
                // TODO
            }
        }
    }

    private void SetPlayerUI()
    {
        // P1 UI set
        CharacterData p1Data = GameManager.instance.player1Data;
        if (p1Data != null)
        {
            p1NameText.text = p1Data.characterName;
            p1HpText.text = p1Data.hp.ToString();
        }

        // P2 UI set
        CharacterData p2Data = GameManager.instance.player2Data;
        if (p2Data != null)
        {
            p2NameText.text = p2Data.characterName;
            p2HpText.text = p2Data.hp.ToString();
        }
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