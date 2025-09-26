using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class ReadyManager : MonoBehaviour
{
    private CharacterData selectedCharacter;
    public TextMeshProUGUI infoText;

    public void SelectCharacter(CharacterData characterData)
    {
        infoText.text =
            "Name: " + characterData.characterName + "\n" +
            "HP: " + characterData.hp + "\n" +
            "MP Speed: " + characterData.mpSpeed + "\n" +
            "Move Speed: " + characterData.moveSpeed + "\n" +
            "Attack Speed: " + characterData.attackSpeed;
        
        selectedCharacter = characterData;
    }

    public void ConfirmCharacter1()
    {
        GameManager.instance.player1Data = selectedCharacter;
        Debug.Log("P1: " + GameManager.instance.player1Data.characterName);
        CheckTransition();
    }

    public void ConfirmCharacter2()
    {
        GameManager.instance.player2Data = selectedCharacter;
        Debug.Log("P2: " + GameManager.instance.player2Data.characterName);
        CheckTransition();
    }

    private void CheckTransition()
    {
        if (GameManager.instance.player1Data != null && GameManager.instance.player2Data != null)
        {
            StartCoroutine(GoToMainSceneAfterDelay());
        }
    }

    private IEnumerator GoToMainSceneAfterDelay()
    {
        yield return new WaitForSeconds(1.0f);
        
        FadeManager.Instance.LoadScene("3.Main");
    }
}
