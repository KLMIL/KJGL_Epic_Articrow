using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatus", menuName = "Enemy/Status")]
public class EnemyStatusSO : ScriptableObject
{
    [Header("Basic Status")]
    public int healthPoint = 10;        // HP: 체력
    public int attack = 2;              // ATK: 공격력
    public int defense = 0;             // DEF: 방어력
    public float moveSpeed = 2.5f;      // SPD: 이동 속도
    public float attackSpeed = 1.0f;    // ASPD: 공격 속도
    public float range = 1.0f;          // RNG: 공격 사거리

    [Header("Special Status")]
    public float detectionRange = 5.0f; // DRNG: 플레이어 감지 범위
    public float knockbackResist = 0f;  // 넉백 저항
}
