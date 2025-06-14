using System.Collections;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public abstract class Projectile : MonoBehaviour
    {
        public SkillManager SkillManager;

        protected int _curPenetration;
        protected abstract int BasePenetration { get; }
        protected abstract float MoveSpeed { get; }
        protected abstract float Damage { get; }
        protected abstract float ExistTime { get; }

        bool _isHit;

        protected void OnEnable()
        {
            _curPenetration = BasePenetration;
            StartCoroutine(DisableCoroutine(ExistTime));
        }

        protected void OnDisable()
        {
            SkillManager = null;
            _isHit = false;
        }

        private void Update()
        {
            transform.position += transform.up * MoveSpeed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.isTrigger) return;
            if (_isHit) return;

            _isHit = true;
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