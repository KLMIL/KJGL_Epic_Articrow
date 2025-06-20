using System.Collections;
using UnityEngine;

namespace CKT
{
    public class Bullet_T3 : Projectile
    {
        LineRenderer _line;
        BoxCollider2D _collider;
        float _width = 0.4f;
        float _distance = 6;

        public override void Init(bool isCreateFromPlayer)
        {
            base.Init(isCreateFromPlayer);

            _line = _line ?? GetComponent<LineRenderer>();
            _line.startWidth = _width;
            _line.SetPosition(0, this.transform.position);
            _line.SetPosition(1, this.transform.position + (this.transform.up * _distance));
            _line.enabled = true;

            _collider = _collider ?? GetComponent<BoxCollider2D>();
            _collider.size = new Vector2(_width, _distance);
            _collider.offset = new Vector2(0, _distance * 0.5f);
            _collider.enabled = true;
        }  

        protected override IEnumerator DisableCoroutine(float existTime)
        {
            yield return null;
            float deltaWidth = _width / ((base._artifactSO.ExistTime <= 0) ? 0.01f : base._artifactSO.ExistTime);
            while (_line.startWidth > 0)
            {
                _line.startWidth -= deltaWidth * Time.deltaTime;
                yield return null;
            }

            _line.enabled = false;
            _collider.enabled = false;
            YSJ.Managers.TestPool.Return(base._artifactSO.ProjectilePoolID, this.gameObject);
            _disableCoroutine = null;
        }
    }
}