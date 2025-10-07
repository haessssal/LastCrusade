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
    [SerializeField] private float hpIncreaseSpeed = 1.5f; 
    [SerializeField] private float hpDecreaseSpeed = 1f;
    [SerializeField] private float mpIncreaseSpeed = 1.5f;
    [SerializeField] private float mpDecreaseSpeed = 1f;

    private System.Collections.Generic.Dictionary<Image, Coroutine> activeCoroutines =
        new System.Collections.Generic.Dictionary<Image, Coroutine>();

    public void SetupPlayerUI(PlayerStats playerStats, CharacterData data)
    {
        stats = playerStats;
        nameText.text = data.characterName;
        hpText.text = stats.CurrentHp.ToString();
        mpBar.fillAmount = 0f;

        PlayerStats.OnHpChanged += UpdateHpBar;
        PlayerStats.OnMpChanged += UpdateMpBar;
    }

    private void OnDestroy()
    {
        PlayerStats.OnHpChanged -= UpdateHpBar;
        PlayerStats.OnMpChanged -= UpdateMpBar;

        foreach (var coroutine in activeCoroutines.Values) if (coroutine != null) StopCoroutine(coroutine);
    }

    private void UpdateHpBar(int index, float ratio)
    {
        if (stats.playerIndex == index)
        {
            float currentFill = hpBar.fillAmount;
            float speed = (ratio > currentFill) ? hpIncreaseSpeed : hpDecreaseSpeed;

            StartBarAnimation(hpBar, ratio, speed);

            hpText.text = Mathf.FloorToInt(stats.CurrentHp).ToString();
        }
    }

    private void UpdateMpBar(int index, float ratio)
    {
        if (stats.playerIndex == index)
        {
            float currentFill = mpBar.fillAmount;
            float speed = (ratio > currentFill) ? mpIncreaseSpeed : mpDecreaseSpeed;
            
            StartBarAnimation(mpBar, ratio, speed);
            
            // TODO: 마나 텍스트 업데이트 ?
        }
    }

    private void StartBarAnimation(Image barImage, float targetRatio, float speed)
    {
        // 이미 실행중인 코루틴  중지
        if (activeCoroutines.ContainsKey(barImage) && activeCoroutines[barImage] != null)
        {
            StopCoroutine(activeCoroutines[barImage]);
        }

        // 새 코루틴 시작하고 Dictionary에 저장
        Coroutine newCoroutine = StartCoroutine(AnimateBar(barImage, targetRatio, speed));
        activeCoroutines[barImage] = newCoroutine;
    }
    
    private IEnumerator AnimateBar(Image barImage, float targetRatio, float speed)
    {
        float currentFill = barImage.fillAmount;
        
        if (Mathf.Approximately(currentFill, targetRatio)) yield break;

        // 증감속도 결정
        float direction = Mathf.Sign(targetRatio - currentFill); // 1.0f: 증가 / -1.0f: 감소

        while (!Mathf.Approximately(barImage.fillAmount, targetRatio))
        {
            currentFill += direction * speed * Time.deltaTime;
            
            // 증가
            if (direction > 0) barImage.fillAmount = Mathf.Min(targetRatio, currentFill);
            // 감소
            else barImage.fillAmount = Mathf.Max(targetRatio, currentFill);

            yield return null;
        }

        barImage.fillAmount = targetRatio;

        // 코루틴 참조 제거
        activeCoroutines.Remove(barImage);
    }
}