using System.Collections.Generic;
using UnityEngine;
using YSJ;

namespace CKT
{
    public class EquipedArtifact_T1 : MonoBehaviour
    {
        GameObject _fieldArtifact;

        int _handID = 0; //0=초기화, 1=왼손, 2=오른손

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ThrowAway();
            }
        }

        void Attack(List<GameObject> list)
        {
            Debug.Log($"{this.transform.parent.name}에서 공격");
            GameObject bullet = YSJ.Managers.Pool.InstPrefab("Bullet", null, this.transform.position);

            Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position).normalized;
            bullet.GetComponent<Rigidbody2D>().AddForce(dir * 10f, ForceMode2D.Impulse);

            //시전 시 효과 적용
            for (int i = 0; i < list.Count; i++)
            {
                ICastEffectable cast = list[i].GetComponent<ICastEffectable>();
                if (cast != null)
                {
                    cast.CastEffect(bullet);
                }
            }
        }

        void ThrowAway()
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