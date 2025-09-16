using UnityEngine;
using UnityEngine.SceneManagement;
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
    }

    public void ConfirmCharacter2()
    {
        GameManager.instance.player2Data = selectedCharacter;
        Debug.Log("P2: " + GameManager.instance.player2Data.characterName);
    }

    public void GoToMainScene()
    {
        if (GameManager.instance.player1Data != null && GameManager.instance.player2Data != null)
        {
            FadeManager.Instance.LoadScene("2.Main");
        }

        else Debug.Log("Both players must select a character");
    }
}
