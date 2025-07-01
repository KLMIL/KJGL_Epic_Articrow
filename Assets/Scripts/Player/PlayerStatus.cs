using System;
using System.Collections;
using UnityEngine;
using TMPro;
using BMC;

namespace YSJ
{
    public class PlayerStatus : MonoBehaviour, IDamagable
    {
        public enum PlayerState
        {
            Idle = 0,
            Move = 1 << 1,
            Attack = 1 << 2,
            Hurt = 1 << 3,
            Die = 1 << 4,
            Stun = 1 << 5,
        }

        public PlayerState CurrentState { get; set; }

        Rigidbody2D _rb;

        [Header("피격")]
        SpriteRenderer _spriteRenderer;
        [field: SerializeField] public bool IsStop { get; private set; }
        public bool IsHurt { get; private set; } // 피격 여부
        float _cameraShakeIntensity = 0.5f;
        float _cameraShakeTime = 0.25f;
        float _invincibleTime = 1f;
        int _colorChangeLoopCount = 20; // 색상 변경 루프 횟수

        [Header("사망")]
        public static Action OnDeadAction;
        public bool IsDead { get; private set; } = false;

        #region 기준 스테이터스
        [Header("기준 스테이터스")]
        float _defaultMaxHealth = 7f;       // 기본 최대 체력
        float _defaultMaxMana = 5f;         // 기본 최대 마나
        float _defaultDashCoolTime = 1f;    // 기본 대시 쿨타임
        float _defaultMoveSpeed = 6f;       // 기본 이동 속도

        public float MaxHealth
        {
            get => _defaultMaxHealth + OffsetMaxHealth;
        }
        public float MaxMana
        {
            get => _defaultMaxMana + OffsetMaxMana;
        }
        public float DashCoolTime
        {
            get => _defaultDashCoolTime - OffsetDashCoolTime;
        }
        
        public float MoveSpeed
        {
            get => _defaultMoveSpeed + OffsetMoveSpeed;
        }
        #endregion

        #region 스테이터스
        [Header("스테이터스")]
        float _health;              // 체력
        float _mana;                // 마나

        public float Health
        {
            get => _health;
            set
            {
                _health = value;
                _health = Mathf.Clamp(_health, 0, MaxHealth);
                UI_InGameEventBus.OnShowBloodCanvas?.Invoke();
                UI_InGameEventBus.OnPlayerHeartUpdate?.Invoke();
            }
        }
        public float Mana
        {
            get
            {
                return _mana = Mathf.Clamp(_mana, 0, MaxMana);
            }
            set
            {
                _mana = value;
                _mana = Mathf.Clamp(_mana, 0, MaxMana);
                UI_InGameEventBus.OnPlayerManaUpdate?.Invoke();
            }
        }
        #endregion

        #region 추가량
        float _offsetMaxHealth;
        float _offsetMaxMana;
        float _offsetDashCoolTime;
        float _offsetMoveSpeed;

        public float OffsetMaxHealth 
        {
            get => _offsetMaxHealth;
            set
            {
                _offsetMaxHealth = value;
                UI_InGameEventBus.OnPlayerHeartUpdate?.Invoke();
            }
        }
        public float OffsetMaxMana 
        {
            get => _offsetMaxMana; 
            set
            {
                _offsetMaxMana = value;
                UI_InGameEventBus.OnPlayerManaUpdate?.Invoke();
            }
        }
        public float OffsetDashCoolTime 
        { 
            get => _offsetDashCoolTime;
            set
            {
                _offsetDashCoolTime = value;
                PlayerManager.Instance.PlayerDash.DashCoolTime = DashCoolTime;
                UI_InGameEventBus.OnPlayerDashCoolTimeMaxValueUpdate?.Invoke(DashCoolTime);
                UI_InGameEventBus.OnPlayerDashCoolTimeSliderValueUpdate?.Invoke(DashCoolTime);
            }
        }
        public float OffsetMoveSpeed
        {
            get => _offsetMoveSpeed;
            set
            {
                _offsetMoveSpeed = value;
            }
        }

        #endregion

        // 테스트용
        Coroutine _stunCoroutine;

        void Update()
        {
            // 테스트용
            if (Input.GetKeyDown(KeyCode.T))
            {
                //StartDebuffCoroutine(PlayerState.Stun, 1f);
                TakeDamage(1f);
                //OffsetMaxHealth += 2f;
                //OffsetDashCoolTime += 0.1f;
                //SpendMana(1f);
            }
        }

        public void Init()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            OnDeadAction += Die;

            Health = MaxHealth;
            Mana = MaxMana;
        }

        #region 체력 관련
        // 체력 회복
        public void RecoverHealth(float amount) => Health += amount;
        #endregion

        #region 마나 관련
        // 마나 재생
        public void RegenerateMana(float amount) => Mana += amount;

        // 마나 소비
        public bool SpendMana(float amount)
        {
            if (Mana < amount)
                return false;

            Mana += -amount;
            return true;
        }
        #endregion

        #region 피해 및 사망 관련

        public void StartDebuffCoroutine(PlayerState playerState, float time)
        {
            if (IsDead || PlayerManager.Instance.PlayerDash.IsDash)
            {
                return;
            }

            Debug.LogError($"{playerState} 걸림");

            if(_stunCoroutine == null)
            {
                _stunCoroutine = StartCoroutine(StunCoroutine(time));
            }
        }

        IEnumerator StunCoroutine(float time)
        {
            IsStop = true;
            yield return new WaitForSeconds(time);
            CurrentState &= ~PlayerState.Stun;
            IsStop = false;
            _stunCoroutine = null;
            Debug.LogError("스턴해제");
        }

        // 피해 받기
        public void TakeDamage(float damage)
        {
            if (IsDead || IsHurt || PlayerManager.Instance.PlayerDash.IsDash)
            {
                return;
            }

            CurrentState |= PlayerState.Hurt;
            
            Health -= damage;
            StartCoroutine(InvincibleCoroutine(_invincibleTime));
            GameManager.Instance.CameraController.ShakeCamera(_cameraShakeIntensity, _cameraShakeTime);

            if (Health <= 0)
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
                    damagedColor.a += (i <_colorChangeLoopCount * 0.5) ? -alphaChange : alphaChange;
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
            CurrentState |= PlayerState.Die;

            // 피격 색상 변경 중지
            StopAllCoroutines();
            _spriteRenderer.color = Color.gray; // 시체 색상

            // 물리 효과 적용 x
            _rb.linearVelocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }
        #endregion

        void OnDestroy()
        {
            OnDeadAction = null;
        }
    }
}