using UnityEngine;

namespace Game.Enemy
{
    // TODO: 추후 몬스터 프리펩 계층구조 변경 이후 제거예정

    public class TestEnemyTakeDamage : MonoBehaviour
    {
        EnemyTakeDamage _enemyTakeDamage;

        public void Init(EnemyTakeDamage enemyTakeDamage)
        {
            _enemyTakeDamage = enemyTakeDamage;
        }

        public void TakeDamage(float damage)
        {
            _enemyTakeDamage.TakeDamage(damage);
        }
    }

}
