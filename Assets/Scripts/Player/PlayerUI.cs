using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image hpBar;
    public Image mpBar;
    public TextMeshProUGUI hpText;

    private PlayerStats stats;
    [SerializeField] private float healthDecreaseSpeed = 1f;
    private Coroutine healthDecreaseCoroutine;

    public void SetupPlayerUI(PlayerStats playerStats, CharacterData data)
    {
        stats = playerStats;
        nameText.text = data.characterName;
        hpText.text = stats.CurrentHp.ToString();

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
        if (stats.playerIndex == index)
        {
            if (healthDecreaseCoroutine != null) StopCoroutine(healthDecreaseCoroutine);
            healthDecreaseCoroutine = StartCoroutine(DecreaseHealthBar(ratio));
            hpBar.fillAmount = ratio;
            hpText.text = Mathf.FloorToInt(stats.CurrentHp).ToString();
        }
    }

    private IEnumerator DecreaseHealthBar(float targetRatio)
    {
        float currentFill = hpBar.fillAmount;
        while (currentFill > targetRatio)
        {
            currentFill -= healthDecreaseSpeed * Time.deltaTime;
            hpBar.fillAmount = Mathf.Max(targetRatio, currentFill);
            yield return null;
        }
    }

    private void UpdateMpBar(int index, float ratio)
    {
        if (stats.playerIndex == index) mpBar.fillAmount = ratio;
    }
}