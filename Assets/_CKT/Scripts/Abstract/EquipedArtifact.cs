using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    public abstract class EquipedArtifact : MonoBehaviour
    {
        protected abstract GameObject _fieldArtifact { get; }
        protected abstract string _prefabName { get; }
        protected abstract float _attackSpeed { get; }

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ThrowAway();
            }
        }

        protected void Init()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponentInChildren<Animator>();

            /*if (this.transform.GetComponentInParent<YSJ.LeftHand>() != null)
            {
                _skillManager = GameManager.Instance.LeftSkillManager;
            }
            else if (this.transform.GetComponentInParent<YSJ.RightHand>() != null)
            {
                _skillManager = GameManager.Instance.RightSkillManager;
                _renderer.flipY = true;
            }*/

            _skillManager = GameManager.Instance.RightSkillManager;
            _skillManager.OnHandActionT1.SingleRegister((list) => Attack(list));
        }

        protected abstract void Attack(List<GameObject> list);

        protected abstract IEnumerator AttackCoroutine(List<GameObject> list);

        void ThrowAway()
        {
            //필드 아티팩트 생성
            GameObject field = Instantiate(_fieldArtifact);
            field.transform.parent = null;
            field.transform.localPosition = this.transform.position + Vector3.down;

            //빈 손으로 초기화
            _skillManager.OnHandActionT1.Unregister((list) => Attack(list));

            Destroy(this.gameObject);
        }
    }
}