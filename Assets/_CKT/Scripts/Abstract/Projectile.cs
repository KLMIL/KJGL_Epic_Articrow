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

            _ignoreLayerMask = LayerMask.GetMask("Ignore Raycast", "Player", "BreakParts");
            _target = null;

            StartCoroutine(ScanTarget());
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

        protected virtual IEnumerator ScanTarget()
        {
            yield return null;
            
            while (_target == null)
            {
                RaycastHit2D hit = Physics2D.CircleCast(this.transform.position, this.transform.localScale.x, Vector2.up, 0, ~_ignoreLayerMask);
                _target = hit.transform;

                if (_target != null)
                {
                    IDamagable iDamageable = _target.GetComponent<IDamagable>();
                    if (iDamageable != null)
                    {
                        iDamageable.TakeDamage(Damage);

                        _curPenetration--;
                        if (_curPenetration < 0)
                        {
                            if (SkillManager != null)
                            {
                                CreateHitSkillObject(this.transform.position, this.transform.up, this.transform.localScale);
                            }

                            if (_disableCoroutine != null)
                            {
                                StopCoroutine(_disableCoroutine);
                            }
                            _disableCoroutine = StartCoroutine(DisableCoroutine(0));
                        }
                    }
                }

                yield return null;
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