using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;

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

    public TextMeshProUGUI winnerText;

    private WorldData currentWorldData;

    private void Start()
    {
        currentWorldData = GameManager.instance.GetCurrentWorldData();
        Debug.Log($"MainManager: Current World = {GameManager.instance.SelectedWorld}");
        winnerText.gameObject.SetActive(false);
        SpawnCharacters();
        ApplyWorldEffect();

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
                CheckWinLose(0); // 무승부
            }
        }
    }

    private void CheckWinLose(int playerIndex)
    {
        if (isMatchOver) return;

        isMatchOver = true;
        if (p1Stats.CurrentHp > 0 && p2Stats.CurrentHp > 0) winnerText.text = "DRAW"; // time over
        else if (p1Stats.CurrentHp <= 0)
        {
            currentWorldData.p2Wins += 1;
            winnerText.text = "WINNER: P2";
        }

        else if (p2Stats.CurrentHp <= 0)
        {
            currentWorldData.p1Wins += 1;
            winnerText.text = "WINNER: P1";
        }

        winnerText.gameObject.SetActive(true);
        currentWorldData.matchCount += 1;

        StartCoroutine(GoToResultSceneAfterDelay());
    }

    private IEnumerator GoToResultSceneAfterDelay()
    {
        yield return new WaitForSeconds(1.0f);

        if (currentWorldData.p1Wins >= 2 || currentWorldData.p2Wins >= 2) FadeManager.Instance.LoadScene("4.Result");
        // 2라운드까지 승패가 결정되지 않았을 경우 3라운드로
        else if (currentWorldData.matchCount < 3) FadeManager.Instance.LoadScene("3.Main");
        // 3라운드까지 진행된 후 최종 결과로 이동
        else FadeManager.Instance.LoadScene("4.Result");
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

            var p1Input = player1Object.GetComponent<PlayerInput>();
            if (p1Input != null)
            {
                p1Input.SwitchCurrentActionMap("Player1");
                Debug.Log("P1: Player1 Action Map");
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

            var p2Input = player2Object.GetComponent<PlayerInput>();
            if (p2Input != null)
            {
                p2Input.SwitchCurrentActionMap("Player2");
                Debug.Log("P2: Player2 Action Map");
            }

            if (p2UI != null) p2UI.SetupPlayerUI(p2Stats, p2Data);
        }
    }

    // test function
    public void P1Demage()
    {
        if (p1Stats != null) p1Stats.TakeDamage(20f);
    }

    public void P2Demage()
    {
        if (p2Stats != null) p2Stats.TakeDamage(20f);
    }

    private void ApplyWorldEffect()
    {
        WorldType selectedWorld = GameManager.instance.SelectedWorld;
        GameObject effectObject = new GameObject(selectedWorld.ToString() + "Effect");
        effectObject.transform.SetParent(this.transform);

        WorldEffect effectComponent = null;

        switch (selectedWorld)
        {
            case WorldType.Ceiling:
                effectComponent = effectObject.AddComponent<CeilingEffect>();
                break;
            case WorldType.Ground:
                effectComponent = effectObject.AddComponent<GroundEffect>();
                break;
            case WorldType.Gravity:
                effectComponent = effectObject.AddComponent<GravityEffect>();
                break;
            case WorldType.Lightning:
                effectComponent = effectObject.AddComponent<LightningEffect>();
                break;
        }

        if (effectComponent != null) effectComponent.Initialize(this);
    
    }
}