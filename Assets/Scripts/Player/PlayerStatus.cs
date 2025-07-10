using UnityEngine;
using BMC;

namespace YSJ
{
    public class PlayerStatus : MonoBehaviour
    {
        public float InteractDistance => _interactDistance;
        float _interactDistance = 0.6f;

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

        #region 기준 스테이터스
        [Header("기준 스테이터스")]
        float _defaultMaxHealth = 6f;       // 기본 최대 체력
        float _defaultMaxMana = 6f;         // 기본 최대 마나
        float _defaultDashCoolTime = 1f;    // 기본 대시 쿨타임
        float _defaultMoveSpeed = 4f;       // 기본 이동 속도

        public float DefaultMaxHealth => _defaultMaxHealth;
        public float DefaultMaxMana => _defaultMaxMana;
        public float DefaultDashCoolTime => _defaultDashCoolTime;
        public float DefaultMoveSpeed => _defaultMoveSpeed;

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
            get => _defaultDashCoolTime + OffsetDashCoolTime;
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
        float _offsetBarrier;
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
                Debug.Log($"{MaxMana}");
                UI_InGameEventBus.OnPlayerManaUpdate?.Invoke();
            }
        }
        public float OffsetBarrier
        {
            get => _offsetBarrier;
            set
            {
                _offsetBarrier = value;
                _offsetBarrier = Mathf.Clamp(_offsetBarrier, 0, int.MaxValue);

                // 배리어는 체력에 영향을 주지 않으면서 보호막 UI 업데이트를 같이 하기 위함
                Health += 0;
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

        void Update()
        {
            TestCode();
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

        #region 테스트 코드

        public void TestCode()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                // 체력 완전 회복
                RecoverHealth(MaxHealth);
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                // 마나 완전 회복
                RegenerateMana(MaxMana);
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                // 보호막 반 칸 획득
                OffsetBarrier += 1f;
            }
            else if(Input.GetKeyDown(KeyCode.F6))
            {
                GameObject testPackage = Managers.Resource.Instantiate("TestArtifactsAndPartsPackage");
                testPackage.transform.position = transform.position;
            }
        }
        #endregion
    }
}