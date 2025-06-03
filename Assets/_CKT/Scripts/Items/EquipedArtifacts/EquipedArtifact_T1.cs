using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    public class EquipedArtifact_T1 : EquipedArtifact
    {
        private void Awake()
        {
            base.Init("FieldArtifacts/FieldArtifact_T1");
        }

        private void Start()
        {
            base.CheckWhichHand();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                base.ThrowAway();
            }
        }

        protected override IEnumerator AttackCoroutine(List<GameObject> list)
        {
            //착용 중인 파츠 효과 확인
            Init("FieldArtifacts/FieldArtifact_T1");
            base.CheckParts(list);

            //시전시 효과 호출
            GameManager.Instance.Inventory.InvokeCastEffect(this.gameObject);

            //마우스 위치 (공격 입력 순간의 마우스 위치 고정)
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //_attackCount만큼 연속 공격
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
                    Vector2 mouseDir = (mousePos - this.transform.position).normalized;
                    Vector2 attackDir = base.RotateVector(mouseDir, (sign * _scatterAngle)).normalized;

                    //총알 생성
                    GameObject bullet = YSJ.Managers.Pool.InstPrefab("Bullet", null, this.transform.position + (Vector3)mouseDir);
                    bullet.GetComponent<Rigidbody2D>().AddForce(attackDir * _bulletSpeed, ForceMode2D.Impulse);
                }

                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(0.5f);
            _attackCoroutine = null;
        }
    }
}