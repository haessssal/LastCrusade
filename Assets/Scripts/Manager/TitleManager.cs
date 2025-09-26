using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void GoToReadyScene()
    {
        FadeManager.Instance.LoadScene("1.Map");
    }
}
