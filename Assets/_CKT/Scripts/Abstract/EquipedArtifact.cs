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

        #region [컴포넌트]
        protected SpriteRenderer _renderer;
        protected Animator _animator;
        #endregion

        float _increaseCoolTimeAmount = 0.5f;

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

            if (this.transform.GetComponentInParent<YSJ.LeftHand>() != null)
            {
                _skillManager = GameManager.Instance.LeftSkillManager;
            }
            else if (this.transform.GetComponentInParent<YSJ.RightHand>() != null)
            {
                _skillManager = GameManager.Instance.RightSkillManager;
                _renderer.flipY = true;
            }

            _skillManager.SingleSubHand((list) => Attack(list));
        }

        protected abstract void Attack(List<GameObject> list);

        protected abstract IEnumerator AttackCoroutine(List<GameObject> list);

        protected float TotalCoolTime(float origin)
        {
            float total = origin;

            if (_skillManager == null)
            {
                Debug.LogWarning("TotalCoolTime Method _skillManager is null");
            }
            else
            {
                int castScatterCount = 0;
                if (_skillManager.SkillDupDict.ContainsKey("CastScatter"))
                {
                    castScatterCount = _skillManager.SkillDupDict["CastScatter"];
                }

                int castAdditionalCount = 0;
                if (_skillManager.SkillDupDict.ContainsKey("CastAdditional"))
                {
                    castAdditionalCount = _skillManager.SkillDupDict["CastAdditional"];
                }

                total = origin * (1 + (_increaseCoolTimeAmount * (castScatterCount + castAdditionalCount)));
            }

            Debug.LogWarning(total);
            return total;
        }

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