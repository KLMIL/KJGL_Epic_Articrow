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
        protected override float _attackSpeed => 0.1f;

        float ChargeAmount
        {
            get
            {
                return _chargeAmount;
            }
            set
            {
                _chargeAmount = value;
                //_img_Charge = _img_Charge ?? GetComponentInChildren<Image>();
                //_img_Charge.fillAmount = _chargeAmount / _maxChargeAmount;
                float amount = Mathf.Clamp01(_chargeAmount / base.TotalCoolTime(_maxChargeAmount));
                _line.startWidth = _maxWidth * amount;
            }
        }
        float _chargeAmount;
        float _maxChargeAmount = 0.8f;

        LineRenderer _line;

        Coroutine _chargeCoroutine;
        Vector3 _firePoint;
        Vector3 _lineStart;
        Vector3 _lineEnd;
        LayerMask _playerLayerMask;
        LayerMask _brokenLayerMask;
        float _distance = 4f;
        float _maxWidth = 0.2f;

        private void Awake()
        {
            _line = _line ?? GetComponent<LineRenderer>();
            _playerLayerMask = LayerMask.GetMask("Player");
            _brokenLayerMask = LayerMask.GetMask("BreakParts");
        }

        #region [Attack]
        protected override void Attack(List<GameObject> list)
        {
            _skillManager.SingleSubHandCancle(() => AttackCancle());

            if (ChargeAmount <= base.TotalCoolTime(_maxChargeAmount))
            {
                if (_attackCoroutine == null)
                {
                    Charge();
                }
            }
            else
            {
                _attackCoroutine = _attackCoroutine ?? StartCoroutine(AttackCoroutine(list));
            }
        }

        void Charge()
        {
            ChargeAmount += Time.fixedDeltaTime;

            _firePoint = this.transform.position + this.transform.up;
            _lineStart = _firePoint;
            _lineEnd = _firePoint + (this.transform.up * _distance);

            float distance = _distance - (_line.startWidth * 0.5f);
            RaycastHit2D hit = Physics2D.CircleCast(_lineStart, _line.startWidth, this.transform.up, distance, ~(_playerLayerMask | _brokenLayerMask));
            if (hit)
            {
                _lineEnd = hit.point;
            }

            _line.SetPosition(0, _lineStart);
            _line.SetPosition(1, _lineEnd);
            _line.enabled = true;

            _chargeCoroutine = _chargeCoroutine ?? StartCoroutine(ChargeCoroutine());
        }

        IEnumerator ChargeCoroutine()
        {
            Debug.DrawLine(_lineStart, _lineEnd, Color.green, 0.4f);
            float distance = _distance - (_line.startWidth * 0.5f);
            RaycastHit2D hit = Physics2D.CircleCast(_lineStart, _line.startWidth, this.transform.up, distance, ~(_playerLayerMask | _brokenLayerMask));
            if (hit)
            {
                IDamagable iDamagable = hit.transform.GetComponent<IDamagable>();
                if (iDamagable != null)
                {
                    iDamagable.TakeDamage(1);
                }

                _lineEnd = hit.point;
            }

            yield return new WaitForSeconds(_attackSpeed);
            _chargeCoroutine = null;
        }

        protected override IEnumerator AttackCoroutine(List<GameObject> list)
        {
            ChargeAmount = 0;
            _line.enabled = false;

            //TODO : 사운드_투사체 발사
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

        #region [Attack Cancle]
        void AttackCancle()
        {
            _skillManager.InitHandCancle();

            ChargeAmount = 0;
            _line.enabled = false;
            Debug.Log("Attack Cancle");
        }
        #endregion
    }
}