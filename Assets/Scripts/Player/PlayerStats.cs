using UnityEngine;
using System;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public static Action<int, float> OnHpChanged;
    public static Action<int, float> OnMpChanged;
    public static Action<int> OnPlayerDeath;

    [SerializeField] private float hp;
    [SerializeField] private float mp;
    [SerializeField] private float mpSpeed;
    public float moveSpeed;
    [SerializeField] private float attackSpeed;

    public int playerIndex;

    private float currentHp;
    private float currentMp;
    private float maxHp;
    private float maxMp;

    private Coroutine mpRegenCoroutine;

    public float CurrentHp => currentHp;
    public float CurrentMp => currentMp;

    public void InitializeStats(CharacterData data, int index)
    {
        hp = data.hp;
        mp = data.mp;
        moveSpeed = data.moveSpeed;
        attackSpeed = data.attackSpeed;
        mpSpeed = data.mpSpeed;

        maxHp = hp;
        maxMp = mp;
        currentHp = maxHp;
        currentMp = 0;
        playerIndex = index;

        OnHpChanged?.Invoke(playerIndex, currentHp / maxHp);
        OnMpChanged?.Invoke(playerIndex, currentMp / maxMp);

        if (mpRegenCoroutine != null) StopCoroutine(mpRegenCoroutine);
        mpRegenCoroutine = StartCoroutine(RegenerateMp());
    }

    public void TakeDamage(float amount)
    {
        currentHp -= amount;
        if (currentHp <= 0)
        {
            currentHp = 0;
            OnPlayerDeath?.Invoke(playerIndex);
        }
        
        OnHpChanged?.Invoke(playerIndex, currentHp / maxHp);
    }

    public void AddHp(float amount)
    {
        currentHp += amount;
        if (currentHp > maxHp) currentHp = maxHp;
        OnHpChanged?.Invoke(playerIndex, currentHp / maxHp);
    }

    public void AddMp(float amount)
    {
        currentMp += amount;
        if (currentMp > maxMp) currentMp = maxMp;
        OnMpChanged?.Invoke(playerIndex, currentMp / maxMp);
    }

    private IEnumerator RegenerateMp()
    {
        while (true)
        {
            if (currentMp < maxMp)
            {
                currentMp += mpSpeed * Time.deltaTime;
                if (currentMp > maxMp) currentMp = maxMp;
                OnMpChanged?.Invoke(playerIndex, currentMp / maxMp);
            }
            
            yield return null;
        }
    }

    public bool CanUseUltimate() => currentMp >= maxMp;

    public void UseUltimate()
    {
        if (CanUseUltimate())
        {
            Debug.Log($"P{playerIndex} used Ultimate!");
            currentMp = 0;
            OnMpChanged?.Invoke(playerIndex, currentMp / maxMp);
        }
    }
}