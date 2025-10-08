using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class ReadyManager : MonoBehaviour
{
    private CharacterData selectedCharacter;
    private GameObject currentPreviewInstance;

    [Header("UI References")]
    public TextMeshProUGUI characterNameText;
    public Transform characterPreviewParent; 
    public StatsVisualizer statsVisualizer;

    [Header("Stat Label Texts")]
    public TextMeshProUGUI[] statLabelTexts = new TextMeshProUGUI[5]; // 스탯명 텍스트
    private readonly string[] STATNAMES = { "HP", "MP", "MP S.", "Move S.", "Attack S." };
    
    [Header("Stat Max Values for Normalization")]
    public float maxHP = 100f;
    public float maxMP = 100f;
    public float maxMpSpeed = 10f;
    public float maxMoveSpeed = 10f;
    public float maxAttackSpeed = 5f;

    public void SelectCharacter(CharacterData characterData)
    {
        selectedCharacter = characterData;

        // 1. 이름 출력
        characterNameText.text = characterData.characterName;
        // 2. prefab 출력
        UpdateCharacterPreview(characterData.characterPrefab);
        // 3. 스탯 graph 출력
        UpdateStatsGraph(characterData);
        // 4. 스탯명 텍스트 출력
        UpdateStatLabels();
    }

    private void UpdateCharacterPreview(GameObject prefab)
    {
        // 이전 프리뷰 제거
        if (currentPreviewInstance != null) Destroy(currentPreviewInstance);

        if (prefab != null && characterPreviewParent != null)
        {
            // 새 프리팹 인스턴스화: 부모의 Transform을 따라감
            currentPreviewInstance = Instantiate(prefab, characterPreviewParent, true);
            
            // 로컬 위치/회전 초기화: 부모의 RectTransform 기준
            currentPreviewInstance.transform.localScale = new Vector3(1f, 1f, 1f); 
            currentPreviewInstance.transform.localPosition = new Vector3(0f, 0f, 0f);
            currentPreviewInstance.transform.localScale = Vector3.one;
            
            // 물리 비활성화: 애니메이션만 돌도록
            Rigidbody2D rb = currentPreviewInstance.GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = false;
        }
    }
    
    private void UpdateStatLabels()
    {
        if (statLabelTexts.Length != STATNAMES.Length) return;

        for (int i = 0; i < STATNAMES.Length; i++) statLabelTexts[i].text = STATNAMES[i];
    }
    
    private void UpdateStatsGraph(CharacterData data)
    {
        if (statsVisualizer == null) return;

        // 1. 각 스탯을 전역 최대값으로 정규화
        float hpRatio = data.hp / maxHP;
        float mpRatio = data.mp / maxMP;
        float mpSpeedRatio = data.mpSpeed / maxMpSpeed;
        float moveSpeedRatio = data.moveSpeed / maxMoveSpeed;
        float attackSpeedRatio = data.attackSpeed / maxAttackSpeed;

        // 2. 그래프 컴포넌트에 데이터 전달 및 UI 업데이트
        statsVisualizer.SetStatValues(
            Mathf.Clamp01(hpRatio),
            Mathf.Clamp01(mpRatio),
            Mathf.Clamp01(mpSpeedRatio),
            Mathf.Clamp01(moveSpeedRatio),
            Mathf.Clamp01(attackSpeedRatio)
        );
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
