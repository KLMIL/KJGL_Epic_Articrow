using System;
using System.Collections;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public abstract class Projectile : MonoBehaviour
    {
        public int CurPenetration
        {
            get => _curPenetration;
            set => _curPenetration = value;
        }
        int _curPenetration;
        protected abstract int BasePenetration { get; }
        protected abstract float MoveSpeed { get; }
        protected abstract float Damage { get; }
        protected abstract float ExistTime { get; }
        protected abstract Define.PoolID PoolID { get; }

        public SkillManager SkillManager;
        protected Coroutine _disableCoroutine;
        Transform _target;

        protected void OnEnable()
        {
            _curPenetration = BasePenetration;

            _disableCoroutine = StartCoroutine(DisableCoroutine(ExistTime));
        }

        protected void OnDisable()
        {
            SkillManager = null;
            _target = null;
        }

        private void FixedUpdate()
        {
            transform.position += transform.up * MoveSpeed * Time.fixedDeltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.isTrigger) return;
            //if (collision.isTrigger || _target != null) return;

            _target = collision.transform;
            IDamagable iDamageable = _target.GetComponent<IDamagable>();
            if (iDamageable != null)
            {
                iDamageable.TakeDamage(Damage);

                //null이 아니면 플레이가 호출한 Projectile,  null이면 HitSkill에서 생성된 Projectile
                if (SkillManager != null)
                {
                    foreach (Func<Vector3, Vector3, IEnumerator> hitSkill in SkillManager.HitSkillDict.Values)
                    {
                        StartCoroutine(hitSkill(collision.ClosestPoint(this.transform.position), this.transform.up));
                    }
                }

                _curPenetration--;
                if (_curPenetration < 0)
                {
                    if (_disableCoroutine != null)
                    {
                        StopCoroutine(_disableCoroutine);
                    }
                    _disableCoroutine = StartCoroutine(DisableCoroutine(0));
                }
            }
        }

        protected virtual IEnumerator DisableCoroutine(float existTime)
        {
            yield return null;
            yield return new WaitForSeconds(existTime);
            YSJ.Managers.TestPool.Return(PoolID, this.gameObject);
            _disableCoroutine = null;
        }
    }
}