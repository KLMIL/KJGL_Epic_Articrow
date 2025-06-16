using System;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class Bullet_T3 : Projectile
    {
        protected override int BasePenetration => 0;
        protected override float MoveSpeed => 0f;
        protected override float Damage => 30f;
        protected override float ExistTime => 0.15f;

        LineRenderer _line;
        BoxCollider2D _collider;
        float _width;
        float _distance;
        float _disappearSpeed;

        protected new void OnEnable()
        {
            _line = _line ?? GetComponent<LineRenderer>();
            _collider = _collider ?? GetComponent<BoxCollider2D>();
            _width = _collider.size.x;
            _distance = _collider.size.y;
            _disappearSpeed = _width / ExistTime;

            base.OnEnable();
            StartCoroutine(LineEnable());
            //StartCoroutine(ScanTarget());
        }

        protected new void OnDisable()
        {
            _line.enabled = false;
            base.OnDisable();
        }

        IEnumerator LineEnable()
        {
            yield return null;
            _line.startWidth = _width;
            _line.SetPosition(0, this.transform.position);
            _line.SetPosition(1, this.transform.position + (this.transform.up * _distance));
            _line.enabled = true;

            while (_line.startWidth > 0)
            {
                _line.startWidth -= _disappearSpeed * Time.deltaTime;
                yield return null;
            }
        }

        /*IEnumerator ScanTarget()
        {
            yield return null;

            Vector3 firePoint = this.transform.position + this.transform.up;
            Vector3 lineStart = firePoint;
            Vector3 lineEnd = firePoint + (this.transform.up * _distance);

            //Debug.DrawLine(lineStart, lineEnd, Color.green, 0.4f);
            float distance = _distance - (_line.startWidth * 0.5f);
            RaycastHit2D[] hits = Physics2D.CircleCastAll(lineStart, _line.startWidth, this.transform.up, distance, ~base._ignoreLayerMask);
            if (hits.Length > 0)
            {
                //for (int i = 0; i < (base._curPenetration + 1); i++)
                for (int i = 0; i < hits.Length; i++)
                {
                    IDamagable iDamagable = hits[i].transform.GetComponent<IDamagable>();
                    if (iDamagable != null)
                    {
                        iDamagable.TakeDamage(Damage);

                        CreateHitSkillObject(hits[i].point, this.transform.up, this.transform.localScale);
                    }
                }

                //lineEnd = hits[base._curPenetration].point;
            }

            _line.SetPosition(0, lineStart);
            _line.SetPosition(1, lineEnd);
            _line.enabled = true;

            while (_line.startWidth > 0)
            {
                _line.startWidth -= Time.deltaTime;
                yield return null;
            }
        }*/
    }
}