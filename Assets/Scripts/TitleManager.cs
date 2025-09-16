using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void GoToReadyScene()
    {
        SceneManager.LoadScene("ReadyScene");
    }
}
