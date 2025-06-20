using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    public class EquipedArtifact_T3 : EquipedArtifact
    {
        float ChargeAmount
        {
            get
            {
                return _chargeAmount;
            }
            set
            {
                _chargeAmount = value;
                float amount = Mathf.Clamp01(_chargeAmount / _maxChargeAmount);
                _line.startWidth = _maxWidth * amount;
            }
        }

        float _chargeAmount;
        float _maxChargeAmount = 0.5f;
        float _chargeSpeed = 0.1f;

        LineRenderer _line;

        Coroutine _chargeCoroutine;
        Vector3 _lineStart;
        Vector3 _lineEnd;
        LayerMask _ignoreLayerMask;
        float _distance = 6f;
        float _maxWidth = 0.1f;

        private void Awake()
        {
            _line = _line ?? GetComponent<LineRenderer>();
            _ignoreLayerMask = LayerMask.GetMask("Default", "Ignore Raycast", "Player", "BreakParts");
        }

        #region [Attack]
        void Charge()
        {
            ChargeAmount += Time.deltaTime;

            _lineStart = base._firePoint.position;
            _lineEnd = base._firePoint.position + (this.transform.up * _distance);

            float distance = _distance - (_line.startWidth * 0.5f);
            RaycastHit2D hit = Physics2D.CircleCast(_lineStart, _line.startWidth, this.transform.up, distance, ~_ignoreLayerMask);
            if (hit)
            {
                _lineEnd = hit.point;
            }

            _line.SetPosition(0, _lineStart);
            _line.SetPosition(1, _lineEnd);
            _line.enabled = true;

            _chargeCoroutine = _chargeCoroutine ?? StartCoroutine(ChargeCoroutine(hit));
        }

        IEnumerator ChargeCoroutine(RaycastHit2D hit)
        {
            Debug.DrawLine(_lineStart, _lineEnd, Color.green, 0.4f);
            if (hit)
            {
                IDamagable iDamagable = hit.transform.GetComponent<IDamagable>();
                if (iDamagable != null)
                {
                    iDamagable.TakeDamage(1);
                }

                _lineEnd = hit.point;
            }

            yield return (_chargeSpeed <= 0) ? null : new WaitForSeconds(_chargeSpeed);
            _chargeCoroutine = null;
        }

        protected override IEnumerator AttackCoroutine(List<GameObject> list)
        {
            while (ChargeAmount <= _maxChargeAmount)
            {
                Charge();
                yield return null;
            }
            
            ChargeAmount = 0;
            _line.enabled = false;

            YSJ.Managers.Sound.PlaySFX(Define.SFX.DefaultAttack);

            //애니메이션 재생
            base._animator.Play("Attack", -1, 0);

            //총알 생성
            GameObject bullet = YSJ.Managers.TestPool.Get<GameObject>(base._artifactSO.ProjectilePoolID);

            bullet.transform.position = base._firePoint.position;
            bullet.transform.up = this.transform.up;
            bullet.GetComponent<Projectile>().Init(true);

            //CastSkill
            foreach (Func<Vector3, Vector3, IEnumerator> castSkill in _skillManager.CastSkillDict.Values)
            {
                StartCoroutine(castSkill(bullet.transform.position, bullet.transform.up));
            }

            yield return new WaitForSeconds(base._artifactSO.AttackCoolTime);
            base._attackCoroutine = null;
        }
        #endregion
    }
}