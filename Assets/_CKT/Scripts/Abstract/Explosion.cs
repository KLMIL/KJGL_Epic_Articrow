using System.Collections;
using UnityEngine;

namespace CKT
{
    public abstract class Explosion : MonoBehaviour
    {
        CircleCollider2D _collider;

        float _totalDamage;
        protected abstract float BaseDamage { get; }
        protected abstract float DisableTime { get; }
        protected abstract Define.PoolID PoolID { get; }

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
        }

        private void OnEnable()
        {
            StartCoroutine(DisableCoroutine());
        }

        public void Init(int level)
        {
            _totalDamage = BaseDamage * level;
        }

        IEnumerator DisableCoroutine()
        {
            yield return null;
            _collider.enabled = true;

            yield return new WaitForSeconds(DisableTime);
            _collider.enabled = false;
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
                Debug.Log("Explosion");
            }
        }
    }
}