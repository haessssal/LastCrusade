using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public void SelectWorld(int worldIndex)
    {
        WorldType selectedWorld = (WorldType)worldIndex;
        GameManager.instance.SetWorld(selectedWorld);

        Debug.Log($"{selectedWorld} world selected");

        FadeManager.Instance.LoadScene("2.Ready");
    }
}
