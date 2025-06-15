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
        float _distance;

        protected new void OnEnable()
        {
            _line = _line ?? GetComponent<LineRenderer>();
            _line.startWidth = 0.2f;

            _distance = 4;

            base.OnEnable();
            StartCoroutine(ScanTarget());
        }

        protected new void OnDisable()
        {
            _line.enabled = false;
            base.OnDisable();
        }

        IEnumerator ScanTarget()
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
                for (int i = 0; i < base._curPenetration + 1; i++)
                {
                    IDamagable iDamagable = hits[i].transform.GetComponent<IDamagable>();
                    if (iDamagable != null)
                    {
                        iDamagable.TakeDamage(Damage);

                        CreateHitSkillObject(hits[i].point, this.transform.up, this.transform.localScale);
                    }
                }

                lineEnd = hits[base._curPenetration].point;
            }

            _line.SetPosition(0, lineStart);
            _line.SetPosition(1, lineEnd);
            _line.enabled = true;

            while (_line.startWidth > 0)
            {
                _line.startWidth -= Time.deltaTime;
                yield return null;
            }
        }
    }
}