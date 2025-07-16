using UnityEngine;

namespace Game.Enemy
{
    [CreateAssetMenu(fileName = "EnemyStatus", menuName = "Enemy/Status")]
    public class EnemyStatusSO : ScriptableObject
    {
        [Header("Info")]
        public Define.EnemyName enemyName = Define.EnemyName.None;

        [Header("Basic Status")]
        public float healthPoint = 10f;     // HP: 체력
        public float attack = 2;              // ATK: 공격력
        public int defence = 0;             // DEF: 방어력
        public float moveSpeed = 2.5f;      // SPD: 이동 속도

        [Header("Special Status")]
        //public float detectionRange = 5.0f; // DRNG: 플레이어 감지 범위
        public float knockbackResist = 0f;  // KRES: 넉백 저항
    }
}
