using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int hp;
    public float mpSpeed;
    public float moveSpeed;
    public float attackSpeed;
    public GameObject characterPrefab; 
}
