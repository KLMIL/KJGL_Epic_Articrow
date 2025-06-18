using System.Collections;
using UnityEngine;

namespace CKT
{
    public class Pellet : Projectile
    {
        public float ScatterSpeed;

        private void FixedUpdate()
        {
            this.transform.localPosition += Vector3.right * ScatterSpeed * Time.fixedDeltaTime;
        }

        protected override IEnumerator DisableCoroutine(float existTime)
        {
            yield return null;
            yield return new WaitForSeconds(existTime);
            this.gameObject.SetActive(false);
            _disableCoroutine = null;
        }
    }
}