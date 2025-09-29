using UnityEngine;

public abstract class WorldEffet : MonoBehaviour
{
    protected MainManager mainManager;

    public virtual void Initialize(MainManager m)
    {
        mainManager = m;
    }
}