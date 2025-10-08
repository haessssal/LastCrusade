using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class ReadyManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerUISection
    {
        public TextMeshProUGUI characterNameText;
        public Transform characterPreviewParent; 
        public StatsVisualizer statsVisualizer;
        public TextMeshProUGUI[] statLabelTexts = new TextMeshProUGUI[5]; // 각 섹션의 스탯명 텍스트
        public CharacterData currentSelectedData; // 현재 선택된 캐릭터 데이터
        [HideInInspector] public GameObject currentPreviewInstance; // 현재 나타내는 프리팹 인스턴스
    }

    [Header("Player UI Sections")]
    public PlayerUISection player1Section; 
    public PlayerUISection player2Section;
    
    private readonly string[] STATNAMES = { "HP", "MP", "MP S.", "Move S.", "Attack S." };
    
    [Header("Stat Max Values for Normalization")]
    public float maxHP = 100f;
    public float maxMP = 100f;
    public float maxMpSpeed = 10f;
    public float maxMoveSpeed = 10f;
    public float maxAttackSpeed = 5f;

    public enum PlayerID { Player1, Player2 }

    public void SelectCharacterP1(CharacterData characterData)
    {
        SelectCharacter(PlayerID.Player1, characterData);
    }

    public void SelectCharacterP2(CharacterData characterData)
    {
        SelectCharacter(PlayerID.Player2, characterData);
    }

    public void SelectCharacter(PlayerID pID, CharacterData characterData)
    {
        PlayerUISection section = (pID == PlayerID.Player1) ? player1Section : player2Section;
        section.currentSelectedData = characterData;

        // 1. 이름 출력
        section.characterNameText.text = section.currentSelectedData.characterName;
        // 2. prefab 출력
        UpdateCharacterPreview(section, section.currentSelectedData.characterPrefab);
        // 3. 스탯 graph 출력
        UpdateStatsGraph(section, section.currentSelectedData);
        // 4. 스탯명 텍스트 출력
        UpdateStatLabels(section);
    }

    private void UpdateCharacterPreview(PlayerUISection pSection, GameObject prefab)
    {
        // 이전 프리뷰 제거
        if (pSection.currentPreviewInstance != null) Destroy(pSection.currentPreviewInstance);

        if (prefab != null && pSection.characterPreviewParent != null)
        {
            // 새 프리팹 인스턴스화: 부모의 Transform을 따라감
            pSection.currentPreviewInstance = Instantiate(prefab, pSection.characterPreviewParent, true);
            
            // 로컬 위치/회전 초기화: 부모의 RectTransform 기준
            pSection.currentPreviewInstance.transform.localScale = new Vector3(1f, 1f, 1f); 
            pSection.currentPreviewInstance.transform.localPosition = new Vector3(0f, 0f, 0f);
            pSection.currentPreviewInstance.transform.localScale = Vector3.one;
            
            // 물리 비활성화: 애니메이션만 돌도록
            Rigidbody2D rb = pSection.currentPreviewInstance.GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = false;
        }
    }
    
    private void UpdateStatLabels(PlayerUISection pSection)
    {
        if (pSection.statLabelTexts.Length != STATNAMES.Length) return;

        for (int i = 0; i < STATNAMES.Length; i++) pSection.statLabelTexts[i].text = STATNAMES[i];
    }
    
    private void UpdateStatsGraph(PlayerUISection pSection, CharacterData data)
    {
        if (pSection.statsVisualizer == null) return;

        // 1. 각 스탯을 전역 최대값으로 정규화
        float hpRatio = data.hp / maxHP;
        float mpRatio = data.mp / maxMP;
        float mpSpeedRatio = data.mpSpeed / maxMpSpeed;
        float moveSpeedRatio = data.moveSpeed / maxMoveSpeed;
        float attackSpeedRatio = data.attackSpeed / maxAttackSpeed;

        // 2. 그래프 컴포넌트에 데이터 전달 및 UI 업데이트
        pSection.statsVisualizer.SetStatValues(
            Mathf.Clamp01(hpRatio),
            Mathf.Clamp01(mpRatio),
            Mathf.Clamp01(mpSpeedRatio),
            Mathf.Clamp01(moveSpeedRatio),
            Mathf.Clamp01(attackSpeedRatio)
        );
    }

    public void ConfirmCharacter1()
    {
        GameManager.instance.player1Data = player1Section.currentSelectedData;
        Debug.Log("P1: " + GameManager.instance.player1Data.characterName);
        CheckTransition();
    }

    public void ConfirmCharacter2()
    {
        GameManager.instance.player2Data = player2Section.currentSelectedData;
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
