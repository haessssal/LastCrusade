using UnityEngine;

// 각각의 아이템에 부착

[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    private ItemManager itemManager;

    public void Init(ItemManager manager)
    {
        itemManager = manager;
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats player = collision.GetComponent<PlayerStats>();
        if (player != null) itemManager.ItemPickedUp(player, gameObject);
    }
}