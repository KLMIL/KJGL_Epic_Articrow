using UnityEngine;
using System.Collections;
using YSJ;

namespace BMC
{
    public class ManaEnergy : MonoBehaviour
    {
        Vector3 _startPos;                  // p1 (시작 지점)
        Vector3 _controlPoint;              // p2 (컨트롤 포인트)
        Vector3 _destinationPos;            // p3 (도착 지점)
        Vector3 _destinationScreenPos;      // p3의 스크린 좌표
        int _upDownDirection;               // p1과 p3의 중점 기준에서의 p2 방향
        float _upDownOffset = 1f;           // p2를 얼마나 띄울지

        Canvas _playerStatusCanvas;
        RectTransform _manaSliderHandle;
        Camera _camera;

        public void Init()
        {
            _camera = GameManager.Instance.Camera;
        }

        public void MoveToSlider()
        {
            // p3 설정
            // UI 월드 좌표 -> Screen 좌표 -> 현재 카메라 기준의 월드 좌표
            _destinationScreenPos = RectTransformUtility.WorldToScreenPoint(_playerStatusCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _camera, _manaSliderHandle.position);
            _destinationPos = _camera.ScreenToWorldPoint(new Vector3(_destinationScreenPos.x, _destinationScreenPos.y, _camera.WorldToScreenPoint(transform.position).z));

            // p1 설정
            _startPos = transform.position;

            // p2 설정
            Vector3 middlePos = (_startPos + _destinationPos) / 2;
            Vector3 perpendicularDir = Vector2.Perpendicular(middlePos).normalized;
            _upDownDirection = Random.Range(-1, 2); // -1, 0, 1 중에 하나 선택
            _controlPoint = middlePos + perpendicularDir * _upDownDirection * _upDownOffset;

            StartCoroutine(MoveBezierCurveToTargetCoroutine());
        }

        // 목표까지 베이지어 곡선으로 이동
        IEnumerator MoveBezierCurveToTargetCoroutine(float duration = 1.0f)
        {
            float time = 0f;
            while (time < 1f)
            {
                // 움직이면 목적지 좌표가 변경되므로 다시 계산
                _destinationScreenPos = RectTransformUtility.WorldToScreenPoint(_playerStatusCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _camera, _manaSliderHandle.position);
                _destinationPos = _camera.ScreenToWorldPoint(new Vector3(_destinationScreenPos.x, _destinationScreenPos.y, _camera.WorldToScreenPoint(transform.position).z));

                // p4(p1과 p2의 사이), p5(p2과 p3의 사이) 설정
                Vector3 p4 = Vector3.Lerp(_startPos, _controlPoint, time);
                Vector3 p5 = Vector3.Lerp(_controlPoint, _destinationPos, time);

                // 이동
                transform.position = Vector3.Lerp(p4, p5, time);
                time += Time.deltaTime / duration;
                yield return null;
            }
            transform.position = _destinationPos;
            Managers.TestPool.Return(Define.PoolID.ManaEnergy, gameObject);
        }
    }
}