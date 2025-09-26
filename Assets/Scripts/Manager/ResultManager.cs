using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public Transform contentParent;
    public GameObject resultPrefab;
    private List<GameObject> resultTextPool = new List<GameObject>(); // 오브젝트 풀링
    private const int MAXRESULTS = 10;
    private WorldData currentWorldData;

    private void Awake()
    {
        currentWorldData = GameManager.instance.GetCurrentWorldData();
        Debug.Log($"ResultManager: Current World = {GameManager.instance.SelectedWorld}");
        RecordGameResult();
        UpdateResultView();
    }

    private void RecordGameResult()
    {
        string result = "";
        if (currentWorldData.p1Wins > currentWorldData.p2Wins || currentWorldData.p1Wins < currentWorldData.p2Wins)
            result = $"{currentWorldData.p1Wins}  :  {currentWorldData.p2Wins}";
        else result = "DRAW";

        currentWorldData.matchResults.Add(result);
    }

    private void UpdateResultView()
    {
        // 기존 풀 결과 초기화
        for (int i = 0; i < resultTextPool.Count; i++) resultTextPool[i].SetActive(false);

        // 현재 월드 결과
        for (int i = 0; i < currentWorldData.matchResults.Count; i++)
        {
            GameObject resultObject;
            if (i < resultTextPool.Count)
            {
                resultObject = resultTextPool[i];
                resultObject.SetActive(true);
            }

            else
            {
                resultObject = Instantiate(resultPrefab, contentParent);
                resultTextPool.Add(resultObject);
            }

            int resultIndex = currentWorldData.matchResults.Count - 1 - i;
            if (resultIndex >= 0)
            {
                TextMeshProUGUI resultText = resultObject.transform.Find("ResultText").GetComponent<TextMeshProUGUI>();
                resultText.text = currentWorldData.matchResults[resultIndex];
            }
        }

        if (currentWorldData.matchResults.Count > MAXRESULTS)
        {
            for (int i = MAXRESULTS; i < resultTextPool.Count; i++)
            {
                resultTextPool[i].SetActive(false);
            }
        }
    }

    private void ResetCurrentWorldData()
    {
        currentWorldData.p1Wins = 0;
        currentWorldData.p2Wins = 0;
        currentWorldData.matchCount = 0;
        // currentWorldData.matchResults.Clear();
    }

    public void GoToReadyScene()
    {
        ResetCurrentWorldData();
        GameManager.instance.player1Data = null;
        GameManager.instance.player2Data = null;

        FadeManager.Instance.LoadScene("2.Ready");
    }

    public void GoToMapScene()
    {
        ResetCurrentWorldData();
        GameManager.instance.player1Data = null;
        GameManager.instance.player2Data = null;

        FadeManager.Instance.LoadScene("1.Map");
    }

    public void GoToTitleScene()
    {
        ResetCurrentWorldData();
        GameManager.instance.player1Data = null;
        GameManager.instance.player2Data = null;

        FadeManager.Instance.LoadScene("0.Title");
    }
}
