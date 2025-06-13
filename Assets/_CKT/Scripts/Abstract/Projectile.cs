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
        Rigidbody2D _rigid;
        Collider2D _collider;

        protected void OnEnable()
        {
            _curPenetration = BasePenetration;

            StartCoroutine(MoveCoroutine());
            StartCoroutine(DisableCoroutine(ExistTime));
        }

        protected void OnDisable()
        {
            SkillManager = null;
            _rigid.linearVelocity = Vector2.zero;
            _collider = null;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.isTrigger || (_collider != null)) return;
            
            _collider = collider;
            //Debug.LogWarning(collider.name);

            IDamagable iDamagable = collider.GetComponent<IDamagable>();
            if (iDamagable != null)
            {
                iDamagable.TakeDamage(Damage);

                _curPenetration--;
                if (_curPenetration < 0)
                {
                    if (SkillManager != null)
                    {
                        CreateHitSkillObject(this.transform.position, this.transform.up, this.transform.localScale);
                    }
                    StartCoroutine(DisableCoroutine(0));
                }

                //TODO : 사운드_투사체 적중
            }
        }

        IEnumerator MoveCoroutine()
        {
            yield return null;
            _rigid = _rigid ?? GetComponent<Rigidbody2D>();
            _rigid.linearVelocity = (Vector2)this.transform.up * MoveSpeed;
        }

        protected IEnumerator DisableCoroutine(float existTime)
        {
            yield return null;
            yield return new WaitForSeconds(existTime);
            //CreateHitSkillObject();
            this.gameObject.SetActive(false);
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