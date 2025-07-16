using UnityEngine;

public class Barrier : MonoBehaviour, IDamagable
{
    public void TakeDamage(float damage, Define.EnemyName attacker = Define.EnemyName.None)
    {
        Destroy(gameObject); // 베리어가 데미지를 받으면 파괴
    }
}
