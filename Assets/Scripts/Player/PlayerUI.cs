using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image hpBar;
    public Image mpBar;

    private PlayerStats stats;

    public void SetupPlayerUI(PlayerStats playerStats, CharacterData data)
    {
        stats = playerStats;
        nameText.text = data.characterName;

        PlayerStats.OnHpChanged += UpdateHpBar;
        PlayerStats.OnMpChanged += UpdateMpBar;
    }

    private void OnDestroy()
    {
        PlayerStats.OnHpChanged -= UpdateHpBar;
        PlayerStats.OnMpChanged -= UpdateMpBar;
    }

    private void UpdateHpBar(int index, float ratio)
    {
        if (stats.playerIndex == index) hpBar.fillAmount = ratio;
    }

    private void UpdateMpBar(int index, float ratio)
    {
        if (stats.playerIndex == index) mpBar.fillAmount = ratio;
    }
}