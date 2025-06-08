using System.Collections;
using UnityEngine;

namespace CKT
{
    public abstract class Projectile : MonoBehaviour
    {
        public SkillManager SkillManager;

        int curPenetration;
        protected abstract int BasePenetration { get; }
        protected abstract float MoveSpeed { get; }
        protected abstract float Damage { get; }
        protected abstract float ExistTime { get; }

        private void OnEnable()
        {
            curPenetration = BasePenetration;
            StartCoroutine(DisableCoroutine());
        }

        private void OnDisable()
        {
            SkillManager = null;
        }

        private void Update()
        {
            transform.position += transform.up * MoveSpeed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            IDamagable iDamagable = collider.GetComponent<IDamagable>();
            if (iDamagable != null)
            {
                iDamagable.TakeDamage(Damage);

                curPenetration--;
                if (curPenetration < 0)
                {
                    CreateHitSkillObject();
                    this.gameObject.SetActive(false);
                }

                //TODO : 사운드_투사체 적중
            }
        }

        IEnumerator DisableCoroutine()
        {
            yield return new WaitForSeconds(ExistTime);
            //CreateHitSkillObject();
            this.gameObject.SetActive(false);
        }

        protected virtual void CreateHitSkillObject()
        {

        }
    }
}