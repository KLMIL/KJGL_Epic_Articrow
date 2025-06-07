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

        private void OnEnable()
        {
            curPenetration = BasePenetration;
            StartCoroutine(DisableCoroutine());
        }

        private void OnDisable()
        {
            SkillManager = null;
        }

        private void FixedUpdate()
        {
            transform.position += transform.up * MoveSpeed * Time.fixedDeltaTime;
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
            }
        }

        IEnumerator DisableCoroutine()
        {
            yield return new WaitForSeconds(0.4f);
            //CreateHitSkillObject();
            this.gameObject.SetActive(false);
        }

        protected virtual void CreateHitSkillObject()
        {

        }
    }
}