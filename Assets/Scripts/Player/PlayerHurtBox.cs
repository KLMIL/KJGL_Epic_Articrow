using UnityEngine;

namespace BMC
{
    // 플레이어 HurtBox
    public class PlayerHurtBox : MonoBehaviour, IDamagable
    {
        public void TakeDamage(float damage)
        {
            PlayerManager.Instance.PlayerStatus.TakeDamage(damage);
        }
    }
}