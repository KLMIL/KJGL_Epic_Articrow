using System.Collections;
using UnityEngine;

namespace CKT
{
    public class Bullet_T2 : Projectile
    {
        SpriteRenderer _renderer;

        public override void Init(bool isCreateFromPlayer)
        {
            base.Init(isCreateFromPlayer);

            _renderer = _renderer ?? GetComponentInChildren<SpriteRenderer>();
            Color color = _renderer.color;
            _renderer.color = new Color(color.r, color.g, color.b, 1);
        }

        protected override IEnumerator DisableCoroutine(float existTime)
        {
            yield return null;

            float deltaAlpha = 1 / ((base._artifactSO.ExistTime <= 0) ? 0.01f : base._artifactSO.ExistTime);
            while (_renderer.color.a > 0)
            {
                _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, _renderer.color.a - ((deltaAlpha * Time.deltaTime)));
                yield return null;
            }

            YSJ.Managers.TestPool.Return(base._artifactSO.ProjectilePoolID, this.gameObject);
            _disableCoroutine = null;
        }
    }
}