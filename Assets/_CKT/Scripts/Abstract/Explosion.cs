using System.Collections;
using UnityEngine;

namespace CKT
{
    public abstract class Explosion : MonoBehaviour
    {
        CircleCollider2D _collider;

        float _totalDamage;
        protected float _disableTime = 0.25f;
        protected abstract Define.PoolID PoolID { get; }

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
        }

        private void OnEnable()
        {
            StartCoroutine(DisableCoroutine());
        }

        public void Init(float damage)
        {
            _totalDamage = damage;
        }

        IEnumerator DisableCoroutine()
        {
            yield return null;
            _collider.enabled = true;
            //Debug.LogError("Explosion");

            yield return (_disableTime <= 0) ? null : new WaitForSeconds(_disableTime);
            _collider.enabled = false;
            YSJ.Managers.Pool.Return(PoolID, this.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.isTrigger) return;

            Transform target = collision.transform;
            IDamagable iDamageable = target.GetComponent<IDamagable>();
            if (iDamageable != null)
            {
                iDamageable.TakeDamage(_totalDamage);
                //Debug.Log("Explosion");
            }
        }
    }
}