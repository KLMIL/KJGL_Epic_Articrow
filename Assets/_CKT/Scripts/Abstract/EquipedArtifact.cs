using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CKT
{
    public abstract class EquipedArtifact : MonoBehaviour
    {
        [SerializeField] protected ArtifactSO _artifactSO;

        #region [컴포넌트]
        protected SpriteRenderer _renderer;
        protected Animator _animator;
        protected Transform _firePoint;
        #endregion

        #region [외부]
        protected SkillManager _skillManager = null;
        #endregion

        #region [값]
        protected Coroutine _attackCoroutine = null;
        Coroutine _manaLackCoroutine = null;
        #endregion

        private void OnEnable()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponentInChildren<Animator>();
            _firePoint = GetComponentInChildren<FirePoint>().transform;

            _skillManager = BMC.PlayerManager.Instance.Inventory.SkillManager;
            _skillManager.GetArtifactSOFuncT0.SingleRegister(() => { return _artifactSO; });
            YSJ.Managers.Input.OnRightHandAction += Attack;
            YSJ.Managers.Input.OnRightHandActionEnd += AttackCancel;
            //_skillManager.OnHandPerformActionT1.SingleRegister((list) => Attack(list));
            //_skillManager.OnHandCancelActionT0.SingleRegister(() => AttackCancel());
            //_skillManager.OnThrowAwayActionT0.SingleRegister(() => ThrowAway());

            YSJ.Managers.UI.OnUpdateImage_ArtifactActionT1.Trigger(_artifactSO.ArtifactSprite);
        }

        private void OnDisable()
        {
            _skillManager.GetArtifactSOFuncT0.Init();
            YSJ.Managers.Input.OnRightHandAction -= Attack;
            YSJ.Managers.Input.OnRightHandActionEnd -= AttackCancel;
            //_skillManager.OnHandPerformActionT1.Unregister((list) => Attack(list));
            //_skillManager.OnHandCancelActionT0.Unregister(() => AttackCancel());
            //_skillManager.OnThrowAwayActionT0.Unregister(() => ThrowAway());

            YSJ.Managers.UI.OnUpdateImage_ArtifactActionT1.Trigger(null);
        }

        protected void Attack()
        {
            if (_attackCoroutine != null) return;

            Debug.Log("[ckt] EquipedArtifact Attack");

            float curMana = BMC.PlayerManager.Instance.PlayerStatus.Mana;
            float totalManaCost = _artifactSO.ManaCost - BMC.PlayerManager.Instance.PlayerStatus.SpendManaOffsetAmount;
            if (curMana < totalManaCost)
            {
                _manaLackCoroutine = _manaLackCoroutine ?? StartCoroutine(ManaLackCoroutine());
                return;
            }
            else
            {
                BMC.PlayerManager.Instance.PlayerStatus.SpendMana(totalManaCost);
                _attackCoroutine = StartCoroutine(AttackCoroutine(_skillManager.SlotList));
            }
        }

        protected virtual IEnumerator AttackCoroutine(List<GameObject> list)
        {
            yield return (_artifactSO.AttackDelay <= 0) ? null : new WaitForSeconds(_artifactSO.AttackDelay);
            
            //사운드, 애니메이션 재생
            YSJ.Managers.Sound.PlaySFX(Define.SFX.DefaultAttack);
            _animator.Play("Attack", -1, 0);

            //총알 생성
            GameObject bullet = YSJ.Managers.TestPool.Get<GameObject>(_artifactSO.ProjectilePoolID);

            bullet.transform.position = _firePoint.position;
            bullet.transform.up = this.transform.up;
            Projectile[] projectiles = bullet.GetComponentsInChildren<Projectile>();
            for (int i = 0; i < projectiles.Length; i++)
            {
                projectiles[i].Init(true);
            }

            //CastSkill
            foreach (Func<Vector3, Vector3, IEnumerator> castSkill in _skillManager.CastSkillDict.Values)
            {
                StartCoroutine(castSkill(bullet.transform.position, bullet.transform.up));
            }

            yield return (_artifactSO.AttackCoolTime <= 0) ? null : new WaitForSeconds(_artifactSO.AttackCoolTime);
            _attackCoroutine = null;
        }

        protected virtual void AttackCancel()
        {

        }

        public void ThrowAway()
        {
            //필드 아티팩트 생성
            GameObject fieldArtifact = Resources.Load<GameObject>($"FieldArtifacts/Field{_artifactSO.ArtifactName}");
            GameObject field = Instantiate(fieldArtifact);
            field.transform.parent = null;
            field.transform.localPosition = this.transform.position + Vector3.down;

            //빈 손으로 초기화
            _skillManager.GetArtifactSOFuncT0.Init();
            YSJ.Managers.Input.OnRightHandAction -= Attack;
            YSJ.Managers.Input.OnRightHandActionEnd -= AttackCancel;
            //_skillManager.OnHandPerformActionT1.Unregister((list) => Attack(list));
            //_skillManager.OnHandCancelActionT0.Unregister(() => AttackCancel());
            //_skillManager.OnThrowAwayActionT0.Unregister(() => ThrowAway());

            YSJ.Managers.UI.OnUpdateImage_ArtifactActionT1.Trigger(null);

            Destroy(this.gameObject);
        }

        IEnumerator ManaLackCoroutine()
        {
            Debug.LogWarning("마나가 부족합니다");
            TextMeshPro manaText = YSJ.Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
            manaText.text = "마나 부족";
            manaText.transform.position = transform.position;

            yield return (_artifactSO.AttackCoolTime <= 0) ? null : new WaitForSeconds(_artifactSO.AttackCoolTime);
            _manaLackCoroutine = null;
        }
    }
}

#if false
            if (this.transform.GetComponentInParent<YSJ.LeftHand>() != null)
            {
                _skillManager = GameManager.Instance.LeftSkillManager;
            }
            else if (this.transform.GetComponentInParent<YSJ.RightHand>() != null)
            {
                _skillManager = GameManager.Instance.RightSkillManager;
                _renderer.flipY = true;
            }
#endif