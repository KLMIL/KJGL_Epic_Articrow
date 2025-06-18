using System.Collections;
using UnityEngine;

namespace CKT
{
    public class Pellet : Projectile
    {
        protected override int BasePenetration => 0;
        protected override float MoveSpeed => 0;
        protected override float Damage => 10f;
        protected override float ExistTime => 1f;
        protected override Define.PoolID PoolID => Define.PoolID.Bullet_T4;
        
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