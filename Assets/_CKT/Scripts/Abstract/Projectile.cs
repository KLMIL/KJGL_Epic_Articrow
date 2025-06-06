using System.Collections;
using UnityEngine;

namespace CKT
{
    public abstract class Projectile : MonoBehaviour
    {
        public SkillManager SkillManager;
        float _bulletSpeed = 25f;

        private void OnEnable()
        {
            StartCoroutine(DisableCoroutine());
        }

        private void OnDisable()
        {
            SkillManager = null;
        }

        private void FixedUpdate()
        {
            transform.position += transform.up * _bulletSpeed * Time.fixedDeltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            IDamagable iDamagable = collider.GetComponent<IDamagable>();
            if (iDamagable != null)
            {
                iDamagable.TakeDamage(10);
                CreateHitSkillObject();
                this.gameObject.SetActive(false);
            }
        }

        IEnumerator DisableCoroutine()
        {
            yield return new WaitForSeconds(0.4f);
            CreateHitSkillObject();
            this.gameObject.SetActive(false);
        }

        protected virtual void CreateHitSkillObject()
        {

        }
    }
}