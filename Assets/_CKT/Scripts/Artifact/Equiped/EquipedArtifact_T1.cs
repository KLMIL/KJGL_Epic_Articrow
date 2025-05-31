using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    public class EquipedArtifact_T1 : MonoBehaviour
    {
        GameObject _fieldArtifact;

        private void Awake()
        {
            _fieldArtifact = Resources.Load<GameObject>("FieldArtifacts/FieldArtifact_T1");
        }

        private void Start()
        {
            // TODO 승준님 코드로 대체
            Debug.Log("테스트1");
            if (this.transform.GetComponentInParent<LeftHand_YSJ>() != null)
            {
                Debug.Log("테스트2");
                GameManager.Instance.Inventory.SubLeftHand((slot) => Attack(slot));
            }
            else if (this.transform.GetComponentInParent<RightHand_YSJ>() != null)
            {
                GameManager.Instance.Inventory.SubRightHand((slot) => Attack(slot));
            }

            if (transform.GetComponentInParent<LeftHand_YSJ>() == null)
                Debug.Log("테스트3");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject equiped = Instantiate(_fieldArtifact);
                equiped.transform.parent = null;
                equiped.transform.localPosition = this.transform.position + Vector3.down;
                Destroy(this.gameObject);
            }
        }

        void Attack(List<GameObject> slot)
        {
            Debug.Log($"{this.transform.parent.name}에서 공격");
            GameObject bullet = YSJ.Managers.Pool.InstPrefab("Bullet", null, this.transform.position);
            bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * 10f, ForceMode2D.Impulse);
        }
    }
}