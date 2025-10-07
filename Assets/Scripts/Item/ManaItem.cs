using UnityEngine;

public class ManaItem : MonoBehaviour, IItem
{
    public int manaAmount = 10;

    public void Use(GameObject user)
    {
        PlayerStats stats = user.GetComponent<PlayerStats>();
        if (stats != null) stats.AddMp(manaAmount);
    }
}