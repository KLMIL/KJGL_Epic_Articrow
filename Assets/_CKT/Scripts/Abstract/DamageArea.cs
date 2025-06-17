using System.Collections;
using UnityEngine;

namespace CKT
{
    public abstract class DamageArea : MonoBehaviour
    {
        CircleCollider2D _collider;

        float _totalDamage;
        protected abstract float BaseDamage { get; }
        protected abstract float DamageGap { get; }
        protected abstract int DamageCount { get; }
        protected abstract Define.PoolID PoolID { get; }

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
        }

        private void OnEnable()
        {
            StartCoroutine(DamageAreaCoroutine());
        }

        public void Init(int level)
        {
            _totalDamage = BaseDamage * level;
        }

        IEnumerator DamageAreaCoroutine()
        {
            for (int i = 0; i < DamageCount; i++)
            {
                yield return null;
                _collider.enabled = true;

                yield return new WaitForSeconds(DamageGap);
                _collider.enabled = false;
            }

            YSJ.Managers.TestPool.Return(PoolID, this.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.isTrigger) return;

            Transform target = collision.transform;
            IDamagable iDamageable = target.GetComponent<IDamagable>();
            if (iDamageable != null)
            {
                iDamageable.TakeDamage(_totalDamage);
            }
        }
    }
}