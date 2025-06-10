using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    public abstract class EquipedArtifact : MonoBehaviour
    {
        GameObject _fieldArtifact;
        string _prefabName;
        SkillManager _skillManager;

        Coroutine _attackCoroutine = null;

        #region [자식 오브젝트]
        Animator _animator;
        #endregion

        private void Start()
        {
            CheckWhichHand();
        }

        protected void Init(string fieldArtifact, string prefab)
        {
            _fieldArtifact = _fieldArtifact ?? Resources.Load<GameObject>(fieldArtifact);
            _prefabName = prefab;
            _skillManager = null;

            _attackCoroutine = null;

            _animator = GetComponentInChildren<Animator>();
        }

        void CheckWhichHand()
        {
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

        void Attack(List<GameObject> list)
        {
            _attackCoroutine = _attackCoroutine ?? StartCoroutine(AttackCoroutine(list));
        }

        protected IEnumerator AttackCoroutine(List<GameObject> list)
        {
            //TODO : 사운드_투사체 발사
            YSJ.Managers.Sound.PlaySFX(Define.SFX.DefaultAttack);

            //애니메이션 재생
            _animator.Play("Attack", -1, 0);

            //총알 생성
            GameObject bullet = CreateBullet(_prefabName, _skillManager);

            //CastSkill
            foreach (Func<GameObject, IEnumerator> castSkill in _skillManager.CastSkillDict.Values)
            {
                StartCoroutine(castSkill(bullet));
            }

            yield return new WaitForSeconds(0.5f);
            _attackCoroutine = null;
        }

        protected abstract GameObject CreateBullet(string prefabName, SkillManager skillManager);

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