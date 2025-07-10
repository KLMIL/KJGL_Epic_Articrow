using System.Collections;
using UnityEngine;
using YSJ;

namespace BMC
{
    public class LaserAttackIndicator : AttackIndicator
    {
        BoxCollider2D _boxCollider;
        LaserHitBox _laserHitBox;

        [Header("길이")]
        float _dist = 30f;

        [Header("시간")]
        float _attackTime = 0.5f;   // 공격 지속 시간
        int _loopCount = 3;         // 반복 횟수
        float _tempFillDuration;    // 초기 확장 시간
        float _secondFillDuration = 0.25f;

        [SerializeField] GameObject _laserPrefab;

        void Awake()
        {
            SetSprite();
            _boxCollider = GetComponentInChildren<BoxCollider2D>();
            _laserHitBox = GetComponentInChildren<LaserHitBox>();
        }

        void Start()
        {
            _target = PlayerManager.Instance.transform;
            _startScale = 0.5f;
            _endScale = 1f;
            _laserHitBox.Damage = 1f;
        }

        protected override IEnumerator ChargeCoroutine()
        {
            if (_target == null)
                yield break;

            _tempFillDuration = _fillDuration;
            for (int i = 0; i < _loopCount; i++)
            {
                // 목표를 바라보게 회전
                Vector3 dir = (_target.position - transform.position).normalized;
                transform.right = dir;

                // 원래 스케일 초기화
                _indicator.transform.localScale = new Vector3(0, _startScale, 1);
                _background.transform.localScale = new Vector3(0, _endScale, 1);
                _indicator.enabled = true;
                _background.enabled = true;

                //_dist = Vector2.Distance(_indicator.transform.position, _target.position) / 2;

                // 1. 길이 확장
                _indicator.transform.localScale = new Vector3(_dist, _endScale, 1);
                _background.transform.localScale = new Vector3(_dist, _endScale, 1);

                // 2. 배경에 딱 채우기
                float timer = 0;
                while (timer < _tempFillDuration)
                {
                    timer += Time.deltaTime;
                    float r = timer / _tempFillDuration;
                    float localScaleY = Mathf.Lerp(_startScale, _endScale, r);
                    _indicator.transform.localScale = new Vector3(_dist, localScaleY, 1);
                    yield return null;
                }
                _indicator.transform.localScale = new Vector3(_dist, _endScale, 1);

                // 공격 실행
                Managers.Sound.PlaySFX(Define.SFX.GolemLaser);
                _background.enabled = false;
                _indicator.enabled = false;
                _boxCollider.enabled = true;
                Color originalColor = _indicator.color;
                //_indicator.color = Color.;

                // 레이저 스폰
                GameObject laserInstance = Instantiate(_laserPrefab, _indicator.transform.position, Quaternion.identity);
                laserInstance.transform.right = dir;
                yield return new WaitForSeconds(_attackTime);

                _boxCollider.enabled = false;
                _indicator.enabled = false;
                _indicator.color = originalColor;
                
                _indicator.transform.localScale = new Vector3(0, _startScale, 1);
                _background.transform.localScale = new Vector3(0, _endScale, 1);
                _tempFillDuration = _secondFillDuration;
            }
            _coroutine = null;
        }
    }
}