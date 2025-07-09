using UnityEngine;

namespace BMC
{
    public class DummyProjectile : MonoBehaviour
    {
        float _damage = 1f;
        [SerializeField] LayerMask _stopLayerMask;

        void Awake()
        {
            _stopLayerMask = LayerMask.GetMask("PlayerHurtBox", "Obstacle");
        }

        // 보스가 데미지 주입하는 메서드
        public void Init(float damage)
        {
            _damage = damage;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if ((_stopLayerMask.value & (1 << collision.gameObject.layer)) != 0)
            {
                if(collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    damagable.TakeDamage(_damage);
                }
                Destroy(gameObject);
            }
        }
    }
}