using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CeilingEffect : WorldEffect
{
    // collapse settings
    [SerializeField] private Transform ceilingObject;
    [SerializeField] private float startHeight = 7f;
    [SerializeField] private float endHeight = 4f;
    [SerializeField] private float collapseDuration = 90f; // 총 붕괴 시간

    // ani settings
    [SerializeField] private float dropDuration = 0.5f; // 한쪽이 쿵 하고 내려오는 시간
    [SerializeField] private float waitBetweenDrops = 1.0f; // 한쪽이 내려온 후 다음 쪽이 내려오기까지 그 사이 시간
    [SerializeField] private float maxRotationAngle = 5f; // 최대 기울임 각도

    private Sequence collapseSequence;
    private float targetY; // 궁극적으로 도달할 Y 위치

    private Vector3 initialPosition;

    public override void Initialize(MainManager m)
    {
        base.Initialize(m);
        Debug.Log("Ceiling Effect created");

        // 고유한 초기화 로직
        if (ceilingObject == null)
        {
            // Tag 로 찾음
            GameObject c = GameObject.FindWithTag("Ceiling");
            if (c != null)
            {
                ceilingObject = c.transform;
                Debug.Log("Ceiling object assigned");
            }

            else return;
        }

        initialPosition = ceilingObject.position;
        ceilingObject.position = new Vector3(initialPosition.x, startHeight, initialPosition.z);

        StartCoroutine(StartCollapse());
    }

    private void Update()
    {
        if (targetY > endHeight)
        {
            // 붕괴 시간에 맞추어 targetY 값을 점진적으로 감소시킴
            float collapseRate = (startHeight - endHeight) / collapseDuration;
        
            targetY -= collapseRate * Time.deltaTime;
            targetY = Mathf.Max(targetY, endHeight); 
        }
    }

    private void OnDestroy()
    {
        if (collapseSequence != null)  collapseSequence.Kill();
    }

    IEnumerator StartCollapse()
    {
        targetY = endHeight;
        yield return new WaitForSeconds(1f);
        StartDropSequence();
    }

    private void StartDropSequence()
    {
        collapseSequence = DOTween.Sequence(); // 새 시퀀스 생성

        // 1. 좌측 하강
        collapseSequence.Append(
            ceilingObject.DOBlendableLocalRotateBy(new Vector3(0, 0, maxRotationAngle), dropDuration)
            .SetEase(Ease.OutBounce) // 쿵
        );

        // 2. 기다림
        collapseSequence.AppendInterval(waitBetweenDrops);

        // 3. 우측 하강
        collapseSequence.Append(
            ceilingObject.DOBlendableLocalRotateBy(new Vector3(0, 0, -maxRotationAngle * 2), dropDuration * 2) // 반대쪽으로 2배 회전
            .SetEase(Ease.OutBounce)
        );

        // 4. 기다림
        collapseSequence.AppendInterval(waitBetweenDrops);

        // 반복
        collapseSequence.SetLoops(-1, LoopType.Restart);
    }
}
