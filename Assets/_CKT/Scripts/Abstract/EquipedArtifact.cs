using BMC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YSJ;

namespace CKT
{
    public abstract class EquipedArtifact : MonoBehaviour
    {
        protected abstract GameObject FieldArtifact { get; }
        protected abstract Define.PoolID PoolID { get; }
        protected abstract float ManaCost { get; }
        protected abstract float MoveSpeed { get; }
        protected abstract float ExistTime { get; }
        protected abstract int Penetration { get; }
        protected abstract float Damage { get; }
        protected abstract float AttackSpeed { get; }

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
        #endregion

        private void Start()
        {
            Init();
        }

        protected void Init()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponentInChildren<Animator>();
            _firePoint = GetComponentInChildren<FirePoint>().transform;

            _skillManager = GameManager.Instance.RightSkillManager;
            _skillManager.GetProjectilePoolID.SingleRegister(() => { return PoolID; });
            _skillManager.GetManaCostFloat.SingleRegister(() => { return ManaCost; });
            _skillManager.GetMoveSpeedFloat.SingleRegister(() => { return MoveSpeed; });
            _skillManager.GetExistTimeFloat.SingleRegister(() => { return ExistTime; });
            _skillManager.GetPenetrationInt.SingleRegister(() => { return Penetration; });
            _skillManager.GetDamageFloat.SingleRegister(() => { return Damage; });

            _skillManager.OnHandPerformActionT1.SingleRegister((list) => Attack(list));
            _skillManager.OnHandCancelActionT0.SingleRegister(() => AttackCancel());
            _skillManager.OnThrowAwayActionT0.SingleRegister(() => ThrowAway());
        }

        protected void Attack(List<GameObject> list)
        {
            if (_attackCoroutine != null) return;

            float totalManaCost = 0;
            Dictionary<string, int> skillDupDict = GameManager.Instance.RightSkillManager.SkillDupDict;
            /*foreach (string key in skillDupDict.Keys)
            {
                int manaCost = 5;
                if ((key == "CastAdditional") || (key == "CastScatter"))
                {
                    manaCost = 10;
                }
                totalManaCost += (manaCost * skillDupDict[key]);
            }*/
            totalManaCost += ManaCost;

            int level = 1;
            foreach (string key in skillDupDict.Keys)
            {
                if (key == "CastAdditional")
                {
                    level += skillDupDict[key];
                }
                else if (key == "CastScatter")
                {
                    level += (skillDupDict[key] * 2);
                }
            }
            GameManager.Instance.RightSkillManager.GetDamageRateFloat.SingleRegister(() => { return (1f / level); });

            if (PlayerManager.Instance.PlayerStatus.Mana < totalManaCost)
            {
                Debug.LogWarning("마나가 부족합니다");
                TextMeshPro manaText = Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
                manaText.text = "마나 부족";
                manaText.transform.position = transform.position;
                return;
            }
            else
            {
                PlayerManager.Instance.PlayerStatus.SpendMana(totalManaCost);
                _attackCoroutine = StartCoroutine(AttackCoroutine(list));
            }
        }

        protected virtual IEnumerator AttackCoroutine(List<GameObject> list)
        {
            YSJ.Managers.Sound.PlaySFX(Define.SFX.DefaultAttack);

            //애니메이션 재생
            _animator.Play("Attack", -1, 0);

            //총알 생성
            GameObject bullet = YSJ.Managers.TestPool.Get<GameObject>(PoolID);

            bullet.transform.position = _firePoint.position;
            bullet.transform.up = this.transform.up;
            Projectile projectile = bullet.GetComponent<Projectile>();
            projectile.SkillManager = _skillManager;

            //CastSkill
            foreach (Func<Vector3, Vector3, IEnumerator> castSkill in _skillManager.CastSkillDict.Values)
            {
                StartCoroutine(castSkill(bullet.transform.position, bullet.transform.up));
            }

            yield return new WaitForSeconds(AttackSpeed);
            _attackCoroutine = null;
        }

        protected virtual void AttackCancel()
        {

        }

        void ThrowAway()
        {
            //필드 아티팩트 생성
            GameObject field = Instantiate(FieldArtifact);
            field.transform.parent = null;
            field.transform.localPosition = this.transform.position + Vector3.down;

            //빈 손으로 초기화
            _skillManager.GetProjectilePoolID.Init();
            _skillManager.GetManaCostFloat.Init();
            _skillManager.GetMoveSpeedFloat.Init();
            _skillManager.GetExistTimeFloat.Init();
            _skillManager.GetDamageFloat.Init();
            _skillManager.GetPenetrationInt.Init();

            _skillManager.OnHandPerformActionT1.Unregister((list) => Attack(list));
            _skillManager.OnHandCancelActionT0.Unregister(() => AttackCancel());
            _skillManager.OnThrowAwayActionT0.Unregister(() => ThrowAway());

            Destroy(this.gameObject);
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