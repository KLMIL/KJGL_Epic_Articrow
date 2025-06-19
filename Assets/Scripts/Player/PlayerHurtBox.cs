using UnityEngine;

namespace BMC
{
    // 플레이어 HurtBox
    public class PlayerHurtBox : MonoBehaviour, IDamagable
    {
        public void TakeDamage(float damage)
        {
            Debug.LogWarning("히트박스에서 데미지 처리");
            PlayerManager.Instance.PlayerStatus.TakeDamage(damage);
        }
    }
}