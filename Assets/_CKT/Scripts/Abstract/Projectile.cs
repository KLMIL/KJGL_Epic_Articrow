using System;
using System.Collections;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public abstract class Projectile : MonoBehaviour
    {
        public int Penetration;
        public float DamageRate;
        
        public SkillManager SkillManager;
        protected Coroutine _disableCoroutine;
        Transform _target;

        protected void OnEnable()
        {
            Penetration = GameManager.Instance.RightSkillManager.GetPenetrationInt.Trigger();

            float existTime = GameManager.Instance.RightSkillManager.GetExistTimeFloat.Trigger();
            _disableCoroutine = StartCoroutine(DisableCoroutine(existTime));
        }

        protected void OnDisable()
        {
            SkillManager = null;
            if (_disableCoroutine != null)
            {
                StopCoroutine(_disableCoroutine);
                _disableCoroutine = null;
            }
            _target = null;
        }

        private void FixedUpdate()
        {
            float moveSpeed = GameManager.Instance.RightSkillManager.GetMoveSpeedFloat.Trigger();
            transform.position += transform.up * moveSpeed * Time.fixedDeltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.isTrigger) return;
            //if (collision.isTrigger || _target != null) return;

            _target = collision.transform;
            IDamagable iDamageable = _target.GetComponent<IDamagable>();
            if (iDamageable != null)
            {
                float damage = GameManager.Instance.RightSkillManager.GetDamageFloat.Trigger();
                iDamageable.TakeDamage(damage * DamageRate);

                //null이 아니면 플레이가 호출한 Projectile,  null이면 HitSkill에서 생성된 Projectile
                if (SkillManager != null)
                {
                    Vector3 closestPoint = collision.ClosestPoint(this.transform.position);
                    foreach (Func<Vector3, Vector3, IEnumerator> hitSkill in SkillManager.HitSkillDict.Values)
                    {
                        StartCoroutine(hitSkill(closestPoint, this.transform.up));
                    }
                }

                Penetration--;
                if (Penetration < 0)
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
            Define.PoolID poolID = GameManager.Instance.RightSkillManager.GetProjectilePoolID.Trigger();
            YSJ.Managers.TestPool.Return(poolID, this.gameObject);
            _disableCoroutine = null;
        }
    }
}