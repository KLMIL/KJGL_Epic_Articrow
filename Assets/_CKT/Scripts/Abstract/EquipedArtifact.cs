using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    public abstract class EquipedArtifact : MonoBehaviour
    {
        GameObject _fieldArtifact;
        string prefabName;

        int _handID = 0; //0=초기화, 1=왼손, 2=오른손
        Coroutine _attackCoroutine = null;

        #region [패시브 효과]
        event Action<GameObject> _onPassiveSkillEvent;
        void SubPassiveSkillEvent(Action<GameObject> newSub)
        {
            _onPassiveSkillEvent += newSub;
        }
        void InvokePassiveSkillEvent(GameObject obj)
        {
            _onPassiveSkillEvent?.Invoke(obj);
        }

        event Func<GameObject, Coroutine> _getPassiveSkillCoroutine;
        void SubPassiveSkillCoroutine(Func<GameObject, Coroutine> newSub)
        {
            _getPassiveSkillCoroutine += newSub;
        }
        void InvokePassiveSkillCoroutine(GameObject obj)
        {
            _getPassiveSkillCoroutine?.Invoke(obj);
        }
        #endregion

        #region [시전 시 효과]
        event Action<GameObject> _onCastSkillEvent;
        void SubCastSkillEvent(Action<GameObject> newSub)
        {
            _onCastSkillEvent += newSub;
        }
        void InvokeCastSkillEvent(GameObject obj)
        {
            _onCastSkillEvent?.Invoke(obj);
        }

        event Func<GameObject, Coroutine> _getCastSkillCoroutine;
        void SubCastSkillCoroutine(Func<GameObject, Coroutine> newSub)
        {
            _getCastSkillCoroutine += newSub;
        }
        void InvokeCastSkillCoroutine(GameObject obj)
        {
            _getCastSkillCoroutine?.Invoke(obj);
        }
        #endregion

        protected void Init(string fieldArtifact, string prefab)
        {
            _fieldArtifact = _fieldArtifact ?? Resources.Load<GameObject>(fieldArtifact);
            prefabName = prefab;

            _onCastSkillEvent = null;
            _getCastSkillCoroutine = null;
        }

        protected void CheckWhichHand()
        {
            SkillManager skillManager = null;
            if (this.transform.GetComponentInParent<LeftHand_YSJ>() != null)
            {
                GameManager.Instance.Inventory.SingleSubLeftHand((list) => Attack(list));
                skillManager = GameManager.Instance.LeftSkillManager;
                _handID = 1;
            }
            else if (this.transform.GetComponentInParent<RightHand_YSJ>() != null)
            {
                GameManager.Instance.Inventory.SingleSubRightHand((list) => Attack(list));
                skillManager = GameManager.Instance.RightSkillManager;
                _handID = 2;
            }

            if (transform.GetComponentInParent<LeftHand_YSJ>() == null)
            {

            }

            if (skillManager != null)
            {
                //패시브 효과 구독


                //시전시 효과 구독
                SubCastSkillEvent((obj) => skillManager.CastScatter(obj));
                SubCastSkillCoroutine((obj) => StartCoroutine(skillManager.CastAdditionalCoroutine(obj)));
                SubCastSkillCoroutine((obj) => StartCoroutine(skillManager.CastExplosionCoroutine(obj)));

                //적중시 효과 구독

            }
        }

        void Attack(List<GameObject> list)
        {
            _attackCoroutine = _attackCoroutine ?? StartCoroutine(AttackCoroutine(list));
        }

        protected virtual IEnumerator AttackCoroutine(List<GameObject> list)
        {
            //총알 생성
            GameObject bullet = YSJ.Managers.Pool.InstPrefab(prefabName);
            bullet.transform.position = this.transform.position;
            //이동 방향
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mousePos - this.transform.position).normalized;
            bullet.transform.up = mouseDir;
            //이름 설정 (복사본 만들 때 이름을 받아서 생성하는 용도)
            bullet.name = prefabName;

            InvokeCastSkillEvent(bullet);
            InvokeCastSkillCoroutine(bullet);

            yield return new WaitForSeconds(0.5f);
            _attackCoroutine = null;
        }

        protected void ThrowAway()
        {
            GameObject equiped = Instantiate(_fieldArtifact);
            equiped.transform.parent = null;
            equiped.transform.localPosition = this.transform.position + Vector3.down;

            if (_handID == 1)
            {
                GameManager.Instance.Inventory.InitLeftHand();
            }
            else if (_handID == 2)
            {
                GameManager.Instance.Inventory.InitRightHand();
            }

            Destroy(this.gameObject);
        }
    }
}