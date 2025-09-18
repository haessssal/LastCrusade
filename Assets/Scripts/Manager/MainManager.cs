using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;
    public PlayerUI p1UI;
    public PlayerUI p2UI;
    private PlayerStats p1Stats;
    private PlayerStats p2Stats;

    public TextMeshProUGUI timerText;
    private float matchTime = 120f;
    private bool isMatchOver = false;

    private void Start()
    {
        SpawnCharacters();

        // 플레이어 죽음 이벤트 구독
        PlayerStats.OnPlayerDeath += CheckWinLose;
    }

    private void OnDestroy()
    {
        PlayerStats.OnPlayerDeath -= CheckWinLose;
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
                CheckDraw();
            }
        }
    }

    private void CheckWinLose(int playerIndex)
    {
        if (isMatchOver) return;

        isMatchOver = true;
        if (playerIndex == 1) Debug.Log("P2 승리!");
        else if (playerIndex == 2) Debug.Log("P1 승리!");
    }

    private void CheckDraw()
    {
        if (p1Stats != null && p2Stats != null)
        {
            if (p1Stats.CurrentHp > 0 && p2Stats.CurrentHp > 0) Debug.Log("무승부!");
            else if (p1Stats.CurrentHp < 0) Debug.Log("P2 승리!");
            else Debug.Log("P1 승리!");
        }
    }

    private void SpawnCharacters()
    {
        // P1 spawn
        CharacterData p1Data = GameManager.instance.player1Data;
        if (p1Data != null)
        {
            GameObject player1Object = Instantiate(p1Data.characterPrefab, player1SpawnPoint.position, Quaternion.identity);
            p1Stats = player1Object.GetComponent<PlayerStats>();
            if (p1Stats != null)
            {
                p1Stats.InitializeStats(p1Data, 1);
                player1Object.GetComponent<Player>().Initialize(p1Stats);
            }

            if (p1UI != null) p1UI.SetupPlayerUI(p1Stats, p1Data);
        }

        // P2 spawn
        CharacterData p2Data = GameManager.instance.player2Data;
        if (p2Data != null)
        {
            GameObject player2Object = Instantiate(p2Data.characterPrefab, player2SpawnPoint.position, Quaternion.identity);
            p2Stats = player2Object.GetComponent<PlayerStats>();
            if (p2Stats != null)
            {
                p2Stats.InitializeStats(p2Data, 2);
                player2Object.GetComponent<Player>().Initialize(p2Stats);
            }

            if (p2UI != null) p2UI.SetupPlayerUI(p2Stats, p2Data);
        }
    }

    // test function
    public void P1Demage()
    {
        if (p1Stats != null) p1Stats.TakeDamage(20f);
    }

    // [수정] P2에게 데미지를 입히는 테스트 함수
    public void P2Demage()
    {
        if (p2Stats != null) p2Stats.TakeDamage(20f);
    }
}