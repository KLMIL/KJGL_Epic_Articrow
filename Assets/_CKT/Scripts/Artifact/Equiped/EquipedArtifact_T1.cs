using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    public class EquipedArtifact_T1 : MonoBehaviour
    {
        GameObject _fieldArtifact;

        int _handID = 0; //0=초기화, 1=왼손, 2=오른손

        Coroutine _attackCoroutine = null;
        float _bulletSpeed = 15f;
        int _attackCount = 1;
        int _scatterCount = 1;
        float _scatterAngle = 15f;

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

        //TODO : Utils에 해당 메소드 넣기
        Vector2 RotateVector(Vector2 vector, float angleDegrees)
        {
            float angleRad = angleDegrees * Mathf.Deg2Rad; // 도를 라디안으로 변환
            float cos = Mathf.Cos(angleRad);
            float sin = Mathf.Sin(angleRad);

            // 2D 벡터 회전 공식 적용
            float newX = vector.x * cos - vector.y * sin;
            float newY = vector.x * sin + vector.y * cos;

            return new Vector2(newX, newY);
        }

        void Attack(List<GameObject> list)
        {
            /*//시전 시 효과 적용
            for (int i = 0; i < list.Count; i++)
            {
                ICastEffectable cast = list[i].GetComponent<ICastEffectable>();
                if (cast != null)
                {
                    cast.CastEffect(bullet);
                }
            }*/

            _attackCoroutine = _attackCoroutine ?? StartCoroutine(AttackCoroutine());
        }

        IEnumerator AttackCoroutine()
        {
            //마우스 방향 (공격 입력 순간의 마우스 위치 고정)
            Vector2 mouseDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position).normalized;

            //_attackCount만큼 반복
            for (int i = 0; i < _attackCount; i++)
            {
                //공격 1회에 _scatterCount만큼 생성
                for (int k = 0; k < _scatterCount; k++)
                {
                    //분산 각도
                    float sign = 0;
                    if ((_scatterCount % 2) == 0)
                    {
                        //_scatterCount가 짝수일때 갈라져서 발사되도록
                        sign = ((k % 2 == 0) ? -1 : 1) * (k + 1) / 2.0f;
                    }
                    else
                    {
                        //_scatterCount가 홀수일 때 (0번 발사체가 가운데) + (양 옆으로 한 개씩)
                        sign = ((k % 2 == 0) ? 1 : -1) * Mathf.Ceil(k / 2.0f);
                    }
                    Vector2 bulletDir = RotateVector(mouseDir, (sign * _scatterAngle));

                    //총알 생성
                    GameObject bullet = YSJ.Managers.Pool.InstPrefab("Bullet", null, this.transform.position);
                    bullet.GetComponent<Rigidbody2D>().AddForce(bulletDir * _bulletSpeed, ForceMode2D.Impulse);
                }

                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(0.5f);
            _attackCoroutine = null;
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