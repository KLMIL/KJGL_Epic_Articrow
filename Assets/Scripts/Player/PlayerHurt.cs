using System;
using System.Collections;
using UnityEngine;
using YSJ;
using static YSJ.PlayerStatus;

namespace BMC
{
    public class PlayerHurt : MonoBehaviour, IDamagable
    {
        Rigidbody2D _rb;
        PlayerStatus _playerStatus;

        [Header("피격")]
        SpriteRenderer _spriteRenderer;
        public bool IsHurt { get; private set; } // 피격 여부
        float _cameraShakeIntensity = 0.5f;
        float _cameraShakeTime = 0.25f;
        float _invincibleTime = 1f;
        int _colorChangeLoopCount = 20; // 색상 변경 루프 횟수

        [Header("사망")]
        public static Action OnDeadAction;
        public bool IsDead { get; private set; } = false;

        public void Init()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerStatus = GetComponent<PlayerStatus>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            OnDeadAction += Die;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                TakeDamage(1f);
            }
        }

        #region 피해 및 사망 관련
        // 피해 받기
        public void TakeDamage(float damage)
        {
            if (IsDead || IsHurt || PlayerManager.Instance.PlayerDash.IsDash)
            {
                return;
            }

            _playerStatus.CurrentState |= PlayerState.Hurt;

            _playerStatus.Health -= damage;
            UI_InGameEventBus.OnShowBloodCanvas?.Invoke();
            StartCoroutine(InvincibleCoroutine(_invincibleTime));
            GameManager.Instance.CameraController.ShakeCamera(_cameraShakeIntensity, _cameraShakeTime);

            if (_playerStatus.Health <= 0)
            {
                OnDeadAction.Invoke();
                UI_InGameEventBus.OnShowGameOverCanvas?.Invoke(); // 게임 오버 화면 표시
            }
        }

        // 무적 코루틴
        IEnumerator InvincibleCoroutine(float second)
        {
            IsHurt = true;

            Color damagedColor = Color.gray;

            float loopCount = 0f;
            float alphaChange = 0.1f;

            while (IsHurt)
            {
                // WaitForFixedUpdate()로 0.02초 대기
                //_colorChangeLoopCount(20)번 반복하여 0.4초 동안 색상 변경
                for (int i = 0; i < _colorChangeLoopCount; i++)
                {
                    damagedColor.a += (i < _colorChangeLoopCount * 0.5) ? -alphaChange : alphaChange;
                    _spriteRenderer.color = damagedColor;

                    // 0.02초 대기
                    yield return new WaitForFixedUpdate();
                    loopCount += 0.02f;

                    if (loopCount >= second)
                    {
                        IsHurt = false;
                        break;
                    }
                }
            }
            _spriteRenderer.color = Color.white; // 색상 복구
        }

        // 사망
        void Die()
        {
            IsDead = true;
            _playerStatus.CurrentState |= PlayerState.Die;

            // 피격 색상 변경 중지
            StopAllCoroutines();
            _spriteRenderer.color = Color.gray; // 시체 색상

            // 물리 효과 적용 x
            _rb.linearVelocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }
        #endregion

        private void OnDestroy()
        {
            OnDeadAction = null;
        }
    }
}