using System.Collections;
using UnityEngine;

namespace CKT
{
    public class Pellet : Projectile
    {
        protected override IEnumerator DisableCoroutine(float existTime)
        {
            yield return (existTime <= 0) ? null : new WaitForSeconds(existTime);
            this.gameObject.SetActive(false);
            _disableCoroutine = null;
        }
    }
}