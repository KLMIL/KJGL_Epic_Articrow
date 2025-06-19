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

        protected new void OnEnable()
        {
            _line = _line ?? GetComponent<LineRenderer>();
            _collider = _collider ?? GetComponent<BoxCollider2D>();

            StartCoroutine(LineEnable());
            StartCoroutine(ColliderEnable());
            base.OnEnable();
        }

        IEnumerator ColliderEnable()
        {
            yield return null;
            _collider.size = new Vector2(_width, _distance);
            _collider.offset = new Vector2(0, _distance * 0.5f);
            _collider.enabled = true;
        }

        IEnumerator LineEnable()
        {
            yield return null;
            _line.startWidth = _width;
            _line.SetPosition(0, this.transform.position);
            _line.SetPosition(1, this.transform.position + (this.transform.up * _distance));
            _line.enabled = true;
        }

        protected override IEnumerator DisableCoroutine(float existTime)
        {
            yield return null;
            while (_line.startWidth > 0)
            {
                _line.startWidth -= (_width / existTime) * Time.deltaTime;
                yield return null;
            }
            _line.enabled = false;
            _collider.enabled = false;
            Define.PoolID poolID = SkillManager.GetProjectilePoolID.Trigger();
            YSJ.Managers.TestPool.Return(poolID, this.gameObject);
            _disableCoroutine = null;
        }
    }
}