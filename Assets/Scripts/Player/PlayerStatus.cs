using System;
using System.Collections;
using UnityEngine;
using TMPro;
using BMC;

namespace YSJ
{
    public class PlayerStatus : MonoBehaviour
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


        [Header("피격")]
        [field: SerializeField] public bool IsStop { get; private set; }

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
                //TakeDamage(1f);
                //OffsetMaxHealth += 2f;
                //OffsetDashCoolTime += 0.1f;
                SpendMana(1f);
            }
        }

        public void Init()
        {
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
    }
}