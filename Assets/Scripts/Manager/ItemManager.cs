using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ItemManager : MonoBehaviour
{
    [Header("Item Settings")]
    public GameObject[] itemPrefabs;
    public Transform[] itemSpawnPoints;

    [Header("UI")]
    public Image itemTimerImage;

    [SerializeField] private float itemSpawnTime = 10f;
    private float currentTime;

    // 이벤트: 아이템 획득 시 호출
    public static Action<PlayerStats, GameObject> OnItemPickedUp;

    private void Start()
    {
        currentTime = 0f;
        StartCoroutine(ItemSpawnTimer());
    }

    private IEnumerator ItemSpawnTimer()
    {
        while (true)
        {
            currentTime += Time.deltaTime;
            if (itemTimerImage != null) itemTimerImage.fillAmount = currentTime / itemSpawnTime;

            if (currentTime >= itemSpawnTime)
            {
                SpawnItem();
                currentTime = 0f;
            }

            yield return null;
        }
    }

    private void SpawnItem()
    {
        if (itemPrefabs.Length == 0 || itemSpawnPoints.Length == 0) return;

        // 무작위 아이템 및 위치
        int itemIndex = UnityEngine.Random.Range(0, itemPrefabs.Length);
        int spawnIndex = UnityEngine.Random.Range(0, itemSpawnPoints.Length);

        GameObject spawned = Instantiate(itemPrefabs[itemIndex], itemSpawnPoints[spawnIndex].position, Quaternion.identity);

        ItemPickup pickup = spawned.GetComponent<ItemPickup>();
        if (pickup == null) spawned.AddComponent<ItemPickup>().Init(this);
    }

    // 이벤트: 아이템 획득 시 호출
    public void ItemPickedUp(PlayerStats player, GameObject item)
    {
        IItem itemComponent = item.GetComponent<IItem>();
        if (itemComponent != null) itemComponent.Use(player.gameObject);
        Destroy(item);
    }
}