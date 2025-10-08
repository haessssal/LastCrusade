using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StatsVisualizer : Graphic
{
    private const int NUMSIDES = 5; // 스탯 개수
    private const int NUMGRIDLEVELS = 4; // 내부 그리드 레벨 수

    [Header("Visual Settings")]
    [SerializeField] private Color areaColor = new Color(0.12f, 0.12f, 0.5f, 0.7f); // 남색
    [SerializeField] private Color outlineColor = Color.black; // 테두리 및 그리드 색상
    [SerializeField] private float outlineThickness = 2.0f; // 테두리 두께
    [SerializeField] private float radius = 150f; // 그래프의 최대 반지름

    // ReadyManager에서 스탯 값 설정할 곳
    [Range(0, 1)] private float[] statValues = new float[NUMSIDES]; 
    
    // ReadyManager에서 스탯 값 설정할 수 있는 공개 메서드
    public void SetStatValues(float hp, float mp, float mpSpeed, float moveSpeed, float attackSpeed)
    {
        statValues[0] = hp;
        statValues[1] = mp;
        statValues[2] = mpSpeed;
        statValues[3] = moveSpeed;
        statValues[4] = attackSpeed;
        
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (statValues.Length != NUMSIDES) return;
        
        float angleStep = 360f / NUMSIDES;
        
         // 1. 스탯 영역 (채우기) 그리기
        DrawStatArea(vh, angleStep);

        // 2. 그리드와 외곽선 그리기
        DrawGridAndOutline(vh, angleStep);
    }
    
    private Vector2 GetPoint(float angle, float currentRadius)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(
            currentRadius * Mathf.Cos(rad - 90 * Mathf.Deg2Rad),
            currentRadius * Mathf.Sin(rad - 90 * Mathf.Deg2Rad)
        );
    }

    // 1. 스탯 영역 (채우기) 그리기
    private void DrawStatArea(VertexHelper vh, float angleStep)
    {
        Vector2 center = Vector2.zero;
        int centerIndex = vh.currentVertCount;
        vh.AddVert(center, areaColor, Vector2.zero); // 중앙 정점

        List<int> areaVertIndices = new List<int>();

        for (int i = 0; i < NUMSIDES; i++)
        {
            float angle = i * angleStep;
            float currentRadius = statValues[i] * radius;

            Vector2 point = GetPoint(angle, currentRadius);

            int pointIndex = vh.currentVertCount;
            vh.AddVert(point, areaColor, Vector2.zero);
            areaVertIndices.Add(pointIndex);
        }

        // 중앙과 각 스탯 포인트를 연결하여 삼각형 메시 구성
        for (int i = 0; i < NUMSIDES; i++)
        {
            int p1 = areaVertIndices[i];
            int p2 = areaVertIndices[(i + 1) % NUMSIDES];
            vh.AddTriangle(centerIndex, p1, p2);
        }
    }

    // 2. 그리드와 외곽선 그리기
    private void DrawGridAndOutline(VertexHelper vh, float angleStep)
    {
        Vector2 center = Vector2.zero;

        // 1. 스탯 축 (중앙에서 꼭짓점까지의 선) 그리기
        for (int i = 0; i < NUMSIDES; i++)
        {
            float angle = i * angleStep;
            Vector2 point = GetPoint(angle, radius); // 최대 반지름 사용
            DrawLine(vh, center, point, outlineThickness, outlineColor);
        }

        // 2. 내부 기준선 (수평선)과 외곽선 그리기
        for (int level = 1; level <= NUMGRIDLEVELS; level++)
        {
            // level=NUM_GRID_LEVELS일 때가 가장 바깥쪽 외곽선이 됨
            float currentRadius = radius * (float)level / NUMGRIDLEVELS;

            List<Vector2> levelPoints = new List<Vector2>();
            for (int i = 0; i < NUMSIDES; i++)
            {
                float angle = i * angleStep;
                levelPoints.Add(GetPoint(angle, currentRadius));
            }

            // 각 레벨의 점들을 연결하여 다각형(오각형) 선 그리기
            for (int i = 0; i < NUMSIDES; i++)
            {
                Vector2 p1 = levelPoints[i];
                Vector2 p2 = levelPoints[(i + 1) % NUMSIDES];
                DrawLine(vh, p1, p2, outlineThickness, outlineColor);
            }
        }
    }

    // 두께가 있는 사각형(선)을 그리는 헬퍼 함수
    private void DrawLine(VertexHelper vh, Vector2 p1, Vector2 p2, float thickness, Color color)
    {
        Vector2 tangent = p2 - p1;
        Vector2 normal = new Vector2(-tangent.y, tangent.x).normalized * (thickness / 2f);

        // 사각형의 4개 정점 계산
        Vector2 v1 = p1 - normal;
        Vector2 v2 = p1 + normal;
        Vector2 v3 = p2 + normal;
        Vector2 v4 = p2 - normal;

        int vertCount = vh.currentVertCount;
        
        // 정점 추가
        vh.AddVert(v1, color, Vector2.zero);
        vh.AddVert(v2, color, Vector2.zero);
        vh.AddVert(v3, color, Vector2.zero);
        vh.AddVert(v4, color, Vector2.zero);

        // 삼각형 구성 (사각형 2개)
        vh.AddTriangle(vertCount, vertCount + 1, vertCount + 2);
        vh.AddTriangle(vertCount + 2, vertCount + 3, vertCount);
    }
    
    protected override void OnValidate()
    {
        base.OnValidate();
        // 에디터에서 값이 변경될 때마다 업데이트
        if (statValues.Length != NUMSIDES) statValues = new float[NUMSIDES];
        SetVerticesDirty();
    }
}