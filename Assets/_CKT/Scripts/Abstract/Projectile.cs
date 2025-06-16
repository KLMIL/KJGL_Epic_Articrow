using System.Collections;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public abstract class Projectile : MonoBehaviour
    {
        protected int _curPenetration;
        protected abstract int BasePenetration { get; }
        protected abstract float MoveSpeed { get; }
        protected abstract float Damage { get; }
        protected abstract float ExistTime { get; }

        public SkillManager SkillManager;
        protected LayerMask _ignoreLayerMask;
        Transform _target;
        Coroutine _disableCoroutine;

        protected void OnEnable()
        {
            _curPenetration = BasePenetration;

            _ignoreLayerMask = LayerMask.GetMask("Default", "Ignore Raycast", "Player", "BreakParts");
            _target = null;

            _disableCoroutine = StartCoroutine(DisableCoroutine(ExistTime));
        }

        protected void OnDisable()
        {
            SkillManager = null;
        }

        private void FixedUpdate()
        {
            transform.position += transform.up * MoveSpeed * Time.fixedDeltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.isTrigger) return;
            //if (collision.isTrigger || _target != null) return;

            _target = collision.transform;
            IDamagable iDamageable = _target.GetComponent<IDamagable>();
            if (iDamageable != null)
            {
                iDamageable.TakeDamage(Damage);

                if (SkillManager != null)
                {
                    CreateHitSkillObject(this.transform.position, this.transform.up, this.transform.localScale);
                }

                /*_curPenetration--;
                if (_curPenetration < 0)
                {
                    if (_disableCoroutine != null)
                    {
                        StopCoroutine(_disableCoroutine);
                    }
                    _disableCoroutine = StartCoroutine(DisableCoroutine(0));
                }*/
            }
        }

        protected IEnumerator DisableCoroutine(float existTime)
        {
            yield return null;
            yield return new WaitForSeconds(existTime);
            this.gameObject.SetActive(false);
            _disableCoroutine = null;
        }

        protected void CreateHitSkillObject(Vector3 postion, Vector3 up, Vector3 scale)
        {
            GameObject hitSkillObject = YSJ.Managers.Pool.InstPrefab("HitSkillObject");
            hitSkillObject.transform.position = postion;
            hitSkillObject.transform.up = up;
            hitSkillObject.transform.localScale = scale;
            hitSkillObject.GetComponent<HitSkillObject>().HitSkill(SkillManager);
        }
    }
}