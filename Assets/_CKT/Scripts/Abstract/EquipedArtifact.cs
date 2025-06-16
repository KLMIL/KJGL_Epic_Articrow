using BMC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YSJ;

namespace CKT
{
    public abstract class EquipedArtifact : MonoBehaviour
    {
        protected abstract GameObject _fieldArtifact { get; }
        protected abstract string _prefabName { get; }
        protected abstract float _attackSpeed { get; }
        protected abstract float _manaCost { get; }

        #region [컴포넌트]
        protected SpriteRenderer _renderer;
        protected Animator _animator;
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

            _skillManager = GameManager.Instance.RightSkillManager;
            _skillManager.OnHandPerformActionT1.SingleRegister((list) => Attack(list));
            _skillManager.OnHandCancelActionT0.SingleRegister(() => AttackCancel());
            _skillManager.OnThrowAwayActionT0.SingleRegister(() => ThrowAway());
        }

        protected void Attack(List<GameObject> list)
        {
            if (_attackCoroutine != null) return;
            
            if (PlayerManager.Instance.PlayerStatus.Mana < _manaCost)
            {
                Debug.LogWarning("마나가 부족합니다");
                return;
            }
            else
            {
                PlayerManager.Instance.PlayerStatus.SpendMana(_manaCost);
                _attackCoroutine = StartCoroutine(AttackCoroutine(list));
            }
        }

        protected abstract IEnumerator AttackCoroutine(List<GameObject> list);

        protected abstract void AttackCancel();

        void ThrowAway()
        {
            //필드 아티팩트 생성
            GameObject field = Instantiate(_fieldArtifact);
            field.transform.parent = null;
            field.transform.localPosition = this.transform.position + Vector3.down;

            //빈 손으로 초기화
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