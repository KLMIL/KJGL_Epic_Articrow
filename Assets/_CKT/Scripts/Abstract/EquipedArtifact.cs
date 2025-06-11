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
        protected SkillManager _skillManager = null;

        protected abstract float _attackSpeed { get; }
        protected Coroutine _attackCoroutine = null;

        #region [자식 오브젝트]
        protected Animator _animator;
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
            _animator = GetComponentInChildren<Animator>();

            if (this.transform.GetComponentInParent<LeftHand_YSJ>() != null)
            {
                _skillManager = GameManager.Instance.LeftSkillManager;
            }
            else if (this.transform.GetComponentInParent<RightHand_YSJ>() != null)
            {
                _skillManager = GameManager.Instance.RightSkillManager;
                GetComponentInChildren<SpriteRenderer>().flipY = true;
            }

            _skillManager.SingleSubHand((list) => Attack(list));
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
            _skillManager.InitHand();

            Destroy(this.gameObject);
        }
    }
}