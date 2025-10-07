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
    [SerializeField] private float waitBetweenDrops = 3.0f; // 한쪽이 내려온 후 다음 쪽이 내려오기까지 그 사이 시간
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
        if (collapseSequence != null) collapseSequence.Kill();
    }
    
    private IEnumerator StartCollapse()
    {
        // 시작 높이
        float leftY = startHeight; // 좌측 끝 Y
        float rightY = startHeight; // 우측 끝 Y

        bool isLeft = true; // 좌측부터 쿵
        float dropAmount = 0.2f; // 쿵할 때 내려가는 높이

        while (true)
        {
            Transform target = ceilingObject;

            if (isLeft)
            {
                // 좌측 하강
                float newY = Mathf.Max(leftY - dropAmount, endHeight);
                leftY = newY;

                target.DOMoveY(newY, dropDuration).SetEase(Ease.OutBounce);
                target.DOLocalRotate(new Vector3(0, 0, maxRotationAngle), dropDuration).SetEase(Ease.OutBounce); // 좌측 기울기
            }

            else
            {
                // 우측 하강
                float newY = Mathf.Max(rightY - dropAmount, endHeight);
                rightY = newY;

                target.DOMoveY(newY, dropDuration).SetEase(Ease.OutBounce);
                target.DOLocalRotate(new Vector3(0, 0, -maxRotationAngle), dropDuration).SetEase(Ease.OutBounce); // 우측 기울기
            }

            isLeft = !isLeft; // 좌우 교대

            // 기다림
            yield return new WaitForSeconds(dropDuration + waitBetweenDrops);
        }
    }

}
