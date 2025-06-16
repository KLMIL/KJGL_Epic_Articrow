using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    public class EquipedArtifact_T3 : EquipedArtifact
    {
        protected override GameObject _fieldArtifact => Resources.Load<GameObject>("FieldArtifacts/FieldArtifact_T3");
        protected override string _prefabName => "Bullet_T3";
        protected override float _attackSpeed => 0.5f;
        protected override float _manaCost => 20f;

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
        Vector3 _firePoint;
        Vector3 _lineStart;
        Vector3 _lineEnd;
        LayerMask _ignoreLayerMask;
        float _distance = 4f;
        float _maxWidth = 0.2f;

        private void Awake()
        {
            _line = _line ?? GetComponent<LineRenderer>();
            _ignoreLayerMask = LayerMask.GetMask("Default", "Ignore Raycast", "Player", "BreakParts");
        }

        #region [Attack]
        void Charge()
        {
            ChargeAmount += Time.deltaTime;

            _firePoint = this.transform.position + this.transform.up;
            _lineStart = _firePoint;
            _lineEnd = _firePoint + (this.transform.up * _distance);

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

            yield return new WaitForSeconds(_chargeSpeed);
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
            GameObject bullet = YSJ.Managers.Pool.InstPrefab(_prefabName);
            bullet.transform.position = this.transform.position;
            //이동 방향
            /*Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mousePos - this.transform.position).normalized;
            bullet.transform.up = mouseDir;*/
            bullet.transform.up = this.transform.up;
            //이름 설정 (복사본 만들 때 이름을 받아서 생성하는 용도)
            bullet.name = _prefabName;
            //왼손||오른손 SkillManager 설정
            bullet.GetComponent<Projectile>().SkillManager = base._skillManager;

            //CastSkill
            foreach (Func<GameObject, IEnumerator> castSkill in _skillManager.CastSkillDict.Values)
            {
                StartCoroutine(castSkill(bullet));
            }

            yield return new WaitForSeconds(_attackSpeed);
            base._attackCoroutine = null;
        }
        #endregion

        #region [Attack Cancel]
        protected override void AttackCancel()
        {
            ChargeAmount = 0;
            _line.enabled = false;
            Debug.Log("Attack Cancel");
        }
        #endregion
    }
}