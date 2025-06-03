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

        protected void Init(string fieldArtifact, string prefab)
        {
            _fieldArtifact = _fieldArtifact ?? Resources.Load<GameObject>(fieldArtifact);
            prefabName = prefab;
        }

        protected void CheckWhichHand()
        {
            // TODO 승준님 코드로 대체
            Debug.Log("테스트1");
            if (this.transform.GetComponentInParent<LeftHand_YSJ>() != null)
            {
                Debug.Log("테스트2");
                GameManager.Instance.Inventory.SingleSubLeftHand((list) => Attack(list));
                _handID = 1;
            }
            else if (this.transform.GetComponentInParent<RightHand_YSJ>() != null)
            {
                GameManager.Instance.Inventory.SingleSubRightHand((list) => Attack(list));
                _handID = 2;
            }

            if (transform.GetComponentInParent<LeftHand_YSJ>() == null)
                Debug.Log("테스트3");
        }

        void Attack(List<GameObject> list)
        {
            _attackCoroutine = _attackCoroutine ?? StartCoroutine(AttackCoroutine(list));
        }

        protected virtual IEnumerator AttackCoroutine(List<GameObject> list)
        {
            //총알 생성
            GameObject bullet = YSJ.Managers.Pool.InstPrefab(prefabName, null, this.transform.position);
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mousePos - this.transform.position).normalized;
            bullet.transform.up = mouseDir;
            bullet.name = prefabName;

            if (_handID == 1)
            {
                GameManager.Instance.LeftSkillManager.CastScatter(bullet);
                StartCoroutine(GameManager.Instance.LeftSkillManager.CastAdditionalCoroutine(bullet));
                StartCoroutine(GameManager.Instance.LeftSkillManager.CastExplosionCoroutine(bullet));
            }
            else if (_handID == 2)
            {
                GameManager.Instance.RightSkillManager.CastScatter(bullet);
                StartCoroutine(GameManager.Instance.RightSkillManager.CastAdditionalCoroutine(bullet));
                StartCoroutine(GameManager.Instance.RightSkillManager.CastExplosionCoroutine(bullet));
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