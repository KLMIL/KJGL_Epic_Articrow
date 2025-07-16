using UnityEngine;

namespace BMC
{
    // 플레이어 HurtBox
    public class PlayerHurtBox : MonoBehaviour, IDamagable
    {
        public void TakeDamage(float damage, Define.EnemyName attacker = Define.EnemyName.None)
        {
            PlayerManager.Instance.PlayerHurt.TakeDamage(damage);
        }
    }
}