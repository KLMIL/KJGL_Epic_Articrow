using UnityEngine;

namespace CKT
{
    public class Bullet_T3 : Projectile
    {
        protected override int BasePenetration => 0;
        protected override float MoveSpeed => 0f;
        protected override float Damage => 4f;
        protected override float ExistTime => 0.15f;

        LineRenderer _line;
        LayerMask _playerLayerMask;
        float _distance = 6f;

        new protected void OnEnable()
        {
            base.OnEnable();
            TakeDamage();
        }

        void TakeDamage()
        {
            _line = _line ?? GetComponent<LineRenderer>();
            _playerLayerMask = LayerMask.GetMask("Player");

            Vector3 firePoint = this.transform.position + this.transform.up;
            Vector3 lineStart = firePoint;
            Vector3 lineEnd = firePoint + (this.transform.up * _distance);

            //Debug.DrawLine(lineStart, lineEnd, Color.green, 0.4f);
            RaycastHit2D[] hits = Physics2D.CircleCastAll(lineStart, _line.startWidth, this.transform.up, _distance, ~_playerLayerMask);
            if (hits.Length > 0)
            {
                for (int i = 0; i < base._curPenetration + 1; i++)
                {
                    IDamagable iDamagable = hits[i].transform.GetComponent<IDamagable>();
                    if (iDamagable != null)
                    {
                        iDamagable.TakeDamage(Damage);

                        GameObject hitSkillObject = YSJ.Managers.Pool.InstPrefab("HitSkillObject");
                        hitSkillObject.transform.position = hits[i].transform.position;
                        hitSkillObject.transform.up = this.transform.up;
                        hitSkillObject.transform.localScale = transform.localScale;
                        hitSkillObject.GetComponent<HitSkillObject>().HitSkill(SkillManager);
                    }
                }

                lineEnd = hits[base._curPenetration].point;
            }

            _line.SetPosition(0, lineStart);
            _line.SetPosition(1, lineEnd);
        }
    }
}