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

    private void Awake()
    {
        RecordGameResult();
        UpdateResultView();
    }

    private void RecordGameResult()
    {
        string result = "";
        if (GameManager.instance.p1Wins > GameManager.instance.p2Wins || GameManager.instance.p1Wins < GameManager.instance.p2Wins)
            result = $"{GameManager.instance.p1Wins}  :  {GameManager.instance.p2Wins}";
        else result = "DRAW";

        GameManager.instance.matchResults.Add(result);
    }

    private void UpdateResultView()
    {
        for (int i = 0; i < GameManager.instance.matchResults.Count; i++)
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

            int resultIndex = GameManager.instance.matchResults.Count - 1 - i;
            if (resultIndex >= 0)
            {
                TextMeshProUGUI resultText = resultObject.transform.Find("ResultText").GetComponent<TextMeshProUGUI>();
                resultText.text = GameManager.instance.matchResults[resultIndex];
            }
        }

        if (GameManager.instance.matchResults.Count > MAXRESULTS)
        {
            for (int i = MAXRESULTS; i < resultTextPool.Count; i++)
            {
                resultTextPool[i].SetActive(false);
            }
        }
    }

    public void GoToReadyScene()
    {
        GameManager.instance.p1Wins = 0;
        GameManager.instance.p2Wins = 0;
        GameManager.instance.matchCount = 0;
        GameManager.instance.player1Data = null;
        GameManager.instance.player2Data = null;

        FadeManager.Instance.LoadScene("1.Ready");
    }

    public void GoToTitleScene()
    {
        GameManager.instance.p1Wins = 0;
        GameManager.instance.p2Wins = 0;
        GameManager.instance.matchCount = 0;
        GameManager.instance.player1Data = null;
        GameManager.instance.player2Data = null;

        FadeManager.Instance.LoadScene("0.Title");
    }
}
