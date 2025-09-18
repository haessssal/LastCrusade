using UnityEngine;

public class HealthItem : MonoBehaviour, IItem
{
    public int healAmount = 20;

    public void Use(GameObject user)
    {
        PlayerStats stats = user.GetComponent<PlayerStats>();
        if (stats != null) stats.AddHp(healAmount);
    }
}