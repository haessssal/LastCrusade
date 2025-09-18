using UnityEngine;

// 아이템 스크립트 작성 후 public class 아이템명 : MonoBehaviour, IItem
// public void Use(GameObject user) 로 함수 작성
public interface IItem
{
    void Use(GameObject user);
}