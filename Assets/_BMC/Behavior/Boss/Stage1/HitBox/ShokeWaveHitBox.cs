using UnityEngine;

namespace BMC
{
    public class ShokeWaveHitBox : MonoBehaviour
    {
        public float Damage { get; set; }

        void OnTriggerStay2D(Collider2D collision)
        {
            if ((1 << collision.gameObject.layer) == LayerMask.GetMask("PlayerHurtBox"))
            {
                if (collision.transform.parent.TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    damagable.TakeDamage(Damage, Define.EnemyName.GolemBoss);
                }
            }
        }
    }
}