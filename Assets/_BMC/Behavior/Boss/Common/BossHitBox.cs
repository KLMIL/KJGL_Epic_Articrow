using UnityEngine;

namespace BMC
{
    /// <summary>
    /// 보스 Hit Box, 추후에 다른 보스 공격에도 재사용 가능하게 이를 상속받아서 하면 될 것 같음
    /// </summary>
    public class BossHitBox : MonoBehaviour
    {
        protected Collider2D _collider;
        protected float _damage;

        public virtual void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        public void Init(float damage)
        {
            _damage = damage;
        }

        public void OnOff(bool isActive)
        {
            _collider.enabled = isActive;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if ((1 << collision.gameObject.layer) == LayerMask.GetMask("PlayerHurtBox"))
            {
                if (collision.transform.parent.TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    damagable.TakeDamage(_damage);
                }
            }
        }
    }
}