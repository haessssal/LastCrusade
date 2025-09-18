using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public GameObject p1WinPanel;
    public GameObject p2WinPanel;

    private void Start()
    {
        p1WinPanel.SetActive(false);
        p2WinPanel.SetActive(false);

        ShowWinnerPanel();
    }

    private void ShowWinnerPanel()
    {
        if (GameManager.instance.p1Wins >= 2) p1WinPanel.SetActive(true);
        else if (GameManager.instance.p2Wins >= 2) p2WinPanel.SetActive(true);
    }

    public void GoToReadyScene()
    {
        GameManager.instance.p1Wins = 0;
        GameManager.instance.p2Wins = 0;

        FadeManager.Instance.LoadScene("1.Ready");
    }
    
    public void GoToTitleScene()
    {
        FadeManager.Instance.LoadScene("0.Title");
    }
}
