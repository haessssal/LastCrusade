using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LightningEffect : WorldEffect
{
    [Header("Lightning Settings")]
    [SerializeField] private float strikeInterval = 5f; // 번개치는 간격
    [SerializeField] private float warningDuration = 5f; // 예고 시간
    [SerializeField] private float lightningVisibleTime = 2f; // 번개치는 시간
    private float lightningYStart = 5f; // 번개 시작 높이: 고정
    private float lightningYEnd = -3.92f; // 번개 도착 높이: 고정
    [SerializeField] private float lightningDamageRadius = 0.8f; // 데미지 범위
    [SerializeField] private float damageAmount = 20f; // 피격 시 데미지

    [Header("Visual Prefabs")]
    private GameObject rainParticlePrefab;
    private GameObject lightningParticlePrefab;
    private GameObject warningPrefab;

    [Header("Ground Range")]
    private float groundMinX = -8.7f;
    private float groundMaxX = 8.7f;

    [Header("Flash Effect")] 
    [SerializeField] private Image flashPanel; 
    [SerializeField] private float minFlashDuration = 0.05f; // 최소 번쩍 시간
    [SerializeField] private float maxFlashDuration = 0.15f; // 최대 번쩍 시간
    [SerializeField] private int minFlashes = 3; 
    [SerializeField] private int maxFlashes = 6;

    private bool isRunning;

    public override void Initialize(MainManager m)
    {
        base.Initialize(m);
        Debug.Log("LightningEffect Initialized");

        rainParticlePrefab = Resources.Load<GameObject>("RainParticlePrefab");
        lightningParticlePrefab = Resources.Load<GameObject>("LightningParticlePrefab");
        warningPrefab = Resources.Load<GameObject>("WarningPrefab");

        flashPanel = GameObject.Find("FlashPanel").GetComponent<Image>();
        if (flashPanel != null)
        {
            Color c = flashPanel.color;
            flashPanel.color = new Color(c.r, c.g, c.b, 0f);
        }

        // 비 파티클 계속 재생
            if (rainParticlePrefab != null) Instantiate(rainParticlePrefab, Vector3.zero, Quaternion.identity);

        StartCoroutine(LightningRoutine());
    }

    private IEnumerator LightningRoutine()
    {
        isRunning = true;

        while (isRunning)
        {
            // 1. 예고 준비: 무작위로 움직임
            GameObject warning = Instantiate(warningPrefab);
            float warningX = Random.Range(groundMinX, groundMaxX);
            Vector2 startPos = new Vector2(Random.Range(groundMinX, groundMaxX), lightningYEnd);
            warning.transform.position = startPos;

            float moveTime = warningDuration * 0.5f;
            float targetX = warningX;

            Sequence moveSeq = DOTween.Sequence();
            moveSeq.Append(warning.transform.DOMoveX(Random.Range(groundMinX, groundMaxX), moveTime / 2f).SetEase(Ease.InOutSine))
                   .Append(warning.transform.DOMoveX(targetX, moveTime / 2f).SetEase(Ease.InOutSine));

            // 2. 예고 이펙트 출력
            SpriteRenderer sr = warning.GetComponent<SpriteRenderer>();
            Sequence blinkSeq = DOTween.Sequence();
            blinkSeq.Append(sr.DOFade(0f, 0.2f))
                    .Append(sr.DOFade(1f, 0.2f))
                    .SetLoops((int)(warningDuration / 0.4f));

            yield return new WaitForSeconds(warningDuration);

            // 번개치기 전 번쩍
            StartCoroutine(FlashRoutine()); 

            // 3. 번개 파티클 생성
            Vector2 strikePoint = new Vector2(targetX, lightningYEnd);
            Vector2 lightningStart = new Vector2(targetX, lightningYStart);
            Quaternion rotation = Quaternion.Euler(90f, -90f, 0f); 
            GameObject lightning = Instantiate(lightningParticlePrefab, lightningStart, rotation);
            Debug.Log("Lightning now");

            var ps = lightning.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();

            // 4. 번개 충돌 판정
            Collider2D[] hits = Physics2D.OverlapCircleAll(strikePoint, lightningDamageRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    Debug.Log("⚡ Player hit by lightning");
                    PlayerStats stats = hit.GetComponent<PlayerStats>();
                    if (stats != null) stats.TakeDamage(damageAmount);
                }
            }

            // 5. 번개 파괴
            Destroy(lightning, lightningVisibleTime);
            Destroy(warning);

            // 6. 다음 번개 대기
            yield return new WaitForSeconds(strikeInterval);
        }
    }

    private IEnumerator FlashRoutine()
    {
        int numFlashes = Random.Range(minFlashes, maxFlashes + 1); // 횟수
        
        for (int i = 0; i < numFlashes; i++)
        {
            // 1. 번쩍
            float flashAlpha = Random.Range(0.5f, 1f); // 불투명도 무작위로 
            float flashOnDuration = Random.Range(minFlashDuration, maxFlashDuration);

            flashPanel.DOFade(flashAlpha, flashOnDuration * 0.2f); 
            yield return new WaitForSeconds(flashOnDuration * 0.2f); // 대기

            // 2. 다시 어둡게 / 투명하게
            float flashOffDuration = Random.Range(minFlashDuration, maxFlashDuration);
            
            flashPanel.DOFade(0f, flashOffDuration * 0.8f); // 천천히 투명하게
            yield return new WaitForSeconds(flashOffDuration * 0.8f + Random.Range(0.01f, 0.1f)); // 다음 번쩍 무작위 대기
        }
    }
}