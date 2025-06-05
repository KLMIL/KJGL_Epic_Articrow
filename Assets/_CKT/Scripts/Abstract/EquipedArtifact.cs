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

        SkillManager _skillManager;
        int _handID; //0=초기화, 1=왼손, 2=오른손

        Coroutine _attackCoroutine = null;

        protected void Init(string fieldArtifact, string prefab)
        {
            _fieldArtifact = _fieldArtifact ?? Resources.Load<GameObject>(fieldArtifact);
            prefabName = prefab;

            _skillManager = null;
            _handID = 0;

            _attackCoroutine = null;
        }

        protected void CheckWhichHand()
        {
            if (this.transform.GetComponentInParent<LeftHand_YSJ>() != null)
            {
                GameManager.Instance.Inventory.SingleSubLeftHand((list) => Attack(list));
                _skillManager = GameManager.Instance.LeftSkillManager;
                _handID = 1;
            }
            else if (this.transform.GetComponentInParent<RightHand_YSJ>() != null)
            {
                GameManager.Instance.Inventory.SingleSubRightHand((list) => Attack(list));
                _skillManager = GameManager.Instance.RightSkillManager;
                _handID = 2;
            }

            if (transform.GetComponentInParent<LeftHand_YSJ>() == null)
            {

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
            bullet.GetComponent<Projectile>().SkillManager = _skillManager;

            //CastSkill
            for (int i = 0; i < _skillManager.CastSkillList.Count; i++)
            {
                StartCoroutine(_skillManager.CastSkillList[i](bullet));
            }

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