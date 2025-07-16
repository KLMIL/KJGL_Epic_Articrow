using UnityEngine;

public class TestEnemy : MonoBehaviour, IDamagable
{
    public void TakeDamage(float damage, Define.EnemyName attacker = Define.EnemyName.None)
    {
        print("Damage : " + damage);
    }
}
