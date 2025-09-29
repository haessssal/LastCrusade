using UnityEngine;

// 각 월드의 Effect 스크립트는 WorldEffect를 상속함
public abstract class WorldEffect : MonoBehaviour
{
    protected MainManager mainManager;

    public virtual void Initialize(MainManager m)
    {
        mainManager = m;
    }
}