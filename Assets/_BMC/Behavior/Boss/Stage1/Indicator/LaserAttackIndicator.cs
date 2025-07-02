using System.Collections;
using UnityEngine;

namespace BMC
{
    public class LaserAttackIndicator : MonoBehaviour
    {
        Transform _target;               // 공격 목표
        SpriteRenderer _indicator;             // 인디케이터
        SpriteRenderer _background;            // 배경
        Coroutine _coroutine;
        BoxCollider2D _boxCollider;

        LaserHitBox _laserHitBox;

        [Header("길이")]
        float _dist = 30f;

        [Header("시간")]
        float _fillDuration = 1f;   // 두께 확장 시간
        float _attackTime = 0.5f;   // 공격 지속 시간
        int _loopCount = 3;         // 반복 횟수

        [Header("두께")]
        float _startThickness = 0.5f; // 시작 두께
        float _endThickness = 1f;   // 최종 두께

        void Awake()
        {
            SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            _indicator = spriteRenderers[0];
            _background = spriteRenderers[1];
            _indicator.enabled = false;
            _background.enabled = false;

            _boxCollider = GetComponentInChildren<BoxCollider2D>();
            _laserHitBox = GetComponentInChildren<LaserHitBox>();
        }

        void Start()
        {
            _laserHitBox.Damage = 1f;
            _target = PlayerManager.Instance.transform;
        }

        public void Init(float time)
        {
            _fillDuration = time;
        }

        public void PlayChargeAndAttack()
        {
            if (_coroutine == null)
                _coroutine = StartCoroutine(ChargeCoroutine());
        }

        private IEnumerator ChargeCoroutine()
        {
            if (_target == null)
                yield break;

            for (int i = 0; i < _loopCount; i++)
            {
                // 방향을 목표로 회전
                Vector3 dir = (_target.position - transform.position).normalized;
                transform.right = dir;

                // 원래 스케일 초기화
                _indicator.transform.localScale = new Vector3(0, _startThickness, 1);
                _background.transform.localScale = new Vector3(0, _endThickness, 1);
                _indicator.enabled = true;
                _background.enabled = true;

                //_dist = Vector2.Distance(_indicator.transform.position, _target.position) / 2;

                // 1. 길이 확장
                _indicator.transform.localScale = new Vector3(_dist, _endThickness, 1);
                _background.transform.localScale = new Vector3(_dist, _endThickness, 1);

                // 2. 배경에 딱 채우기
                float timer = 0;
                while (timer < _fillDuration)
                {
                    timer += Time.deltaTime;
                    float r = timer / _fillDuration;
                    float localScaleY = Mathf.Lerp(_startThickness, _endThickness, r);
                    _indicator.transform.localScale = new Vector3(_dist, localScaleY, 1);
                    yield return null;
                }
                _indicator.transform.localScale = new Vector3(_dist, _endThickness, 1);

                // 공격 실행
                _background.enabled = false;
                _indicator.enabled = true;
                _boxCollider.enabled = true;
                Color originalColor = _indicator.color;
                _indicator.color = Color.white;

                yield return new WaitForSeconds(_attackTime);

                _boxCollider.enabled = false;
                _indicator.enabled = false;
                _indicator.color = originalColor;
                _coroutine = null;

                _fillDuration = 0.25F;
            }
        }
    }
}