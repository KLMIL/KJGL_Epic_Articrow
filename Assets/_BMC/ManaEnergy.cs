using UnityEngine;
using System.Collections;
using YSJ;

namespace BMC
{
    public class ManaEnergy : MonoBehaviour
    {
        Vector3 _startPos;                  // p1 (시작 지점)
        Vector3 _controlPoint;              // p2 (컨트롤 포인트)
        Transform _destinationTransform;    // p3 (도착 지점)
        int _upDownDirection;               // p1과 p3의 중점 기준에서의 p2 방향
        float _upDownOffset = 1f;           // p2를 얼마나 띄울지

        void Awake()
        {
            Init();
        }

        public void Init()
        {
            _startPos = Vector3.zero;
            _destinationTransform = null;
        }

        // 봉인 설정
        public void SetSealed(Transform collector = null)
        {
            // p3 설정
            _destinationTransform = collector;

            // p1 설정
            _startPos = transform.position;

            // p2 설정
            Vector3 middlePos = (_startPos + _destinationTransform.position) / 2;
            Vector3 perpendicularDir = Vector2.Perpendicular(middlePos).normalized;
            _upDownDirection = Random.Range(-1, 2); // -1, 0, 1 중에 하나 선택
            _controlPoint = middlePos + perpendicularDir * _upDownDirection * _upDownOffset;

            StartCoroutine(MoveBezierCurveToTargetCoroutine());
        }

        // 목표까지 베이지어 곡선으로 이동
        IEnumerator MoveBezierCurveToTargetCoroutine(float duration = 1.0f)
        {
            float time = 0f;
            while (_destinationTransform != null && Vector2.Distance(transform.position, _destinationTransform.position) >= 0.1f)
            {
                // p4(p1과 p2의 사이), p5(p2과 p3의 사이) 설정
                Vector3 p4 = Vector3.Lerp(_startPos, _controlPoint, time);
                Vector3 p5 = Vector3.Lerp(_controlPoint, _destinationTransform.position, time);

                // 이동
                transform.position = Vector3.Lerp(p4, p5, time);
                time += Time.deltaTime / duration;
                yield return null;
            }
            Managers.TestPool.Return(Define.PoolID.ManaEnergy, gameObject);
        }
    }
}