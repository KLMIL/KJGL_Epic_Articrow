using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    public abstract class EquipedArtifact : MonoBehaviour
    {
        GameObject _fieldArtifact;

        int _handID = 0; //0=초기화, 1=왼손, 2=오른손

        float _baseBulletSpeed = 15f;
        int _baseAttackCount = 1;
        int _baseScatterCount = 1;
        float _baseScatterAngle = 10f;

        protected float _bulletSpeed;
        protected int _attackCount;
        protected int _scatterCount;
        protected float _scatterAngle;

        protected Coroutine _attackCoroutine = null;

        protected void Init(string name)
        {
            _fieldArtifact = _fieldArtifact ?? Resources.Load<GameObject>(name);

            _bulletSpeed = _baseBulletSpeed;
            _attackCount = _baseAttackCount;
            _scatterCount = _baseScatterCount;
            _scatterAngle = _baseScatterAngle;
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

        protected abstract IEnumerator AttackCoroutine(List<GameObject> list);

        /// <summary>
        /// 파츠 효과들을 공격 1회마다 초기화 후 다시 구독,
        /// 구독한 파츠 효과들은 Inventory.Invoke" "Effect(obj)에서 호출해야함
        /// </summary>
        /// <param name="list"></param>
        protected void CheckParts(List<GameObject> list)
        {
            GameManager.Instance.Inventory.InitCastEffect();
            GameManager.Instance.Inventory.InitHitEffect();
            //TODO : 히트박스 게임오브젝트(bullet 등)가 활성화 되어있는 동안 호출될 함수 "초기화"
            //TODO : 히트박스가 충돌하면 1회 호출될 함수 "초기화"

            for (int i = 0; i < list.Count; i++)
            {
                //시전 시 효과 구독
                ICastEffectable cast = list[i].GetComponent<ICastEffectable>();
                if (cast != null)
                {
                    GameManager.Instance.Inventory.SubCastEffect((artifact) => cast.CastEffect(artifact));
                }

                //TODO : 히트박스 게임오브젝트(bullet 등)가 활성화 되어있는 동안 호출될 함수 구독

                //TODO : 히트박스가 충돌하면 1회 호출될 함수 구독
                IHitEffectable hit = list[i].GetComponent<IHitEffectable>();
                if (hit != null)
                {
                    GameManager.Instance.Inventory.SubHitEffect((hitBox) => hit.HitEffect(hitBox));
                }
            }
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

        #region [ICastEffectable, IHitEffectable에서 호출할 Method]
        public void AddScatterCount(int add)
        {
            _scatterCount += add;
        }

        public void AddAttackCount(int add)
        {
            _attackCount += add;
        }
        #endregion

        #region [Utils]
        //TODO : Utils에 해당 메소드 넣기
        protected Vector2 RotateVector(Vector2 vector, float angleDegrees)
        {
            float angleRad = angleDegrees * Mathf.Deg2Rad; // 도를 라디안으로 변환
            float cos = Mathf.Cos(angleRad);
            float sin = Mathf.Sin(angleRad);

            // 2D 벡터 회전 공식 적용
            float newX = vector.x * cos - vector.y * sin;
            float newY = vector.x * sin + vector.y * cos;

            return new Vector2(newX, newY);
        }
        #endregion
    }
}