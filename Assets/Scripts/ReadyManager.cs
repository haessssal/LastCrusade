using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadyManager : MonoBehaviour
{
    public CharacterData character1;
    public CharacterData character2;

    public void GoToMainScene()
    {
        if (character1 != null && character2 != null)
        {
            GameManager.instance.player1Data = character1;
            GameManager.instance.player2Data = character2;

            Debug.Log("P1: " + character1.characterName + "\n" + "P2: " + character2.characterName);

            SceneManager.LoadScene("2.Main");
        }
    }
}
