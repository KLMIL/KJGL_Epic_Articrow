using System;
using UnityEngine;
using TMPro;
using BMC;

namespace YSJ
{
    public class PlayerStatus : MonoBehaviour, IDamagable
    {
        public Action OnDeadAction;

        public bool IsDead { get; private set; } = false;

        [Header("업그레이드 가능한 스테이터스")]
        public float MaxHealth { get; set; } = 100;
        public float MaxMana { get; set; } = 100;
        public int MoveSpeed { get; set; } = 10;
        public int AttackPoint { get; set; } = 10;
        public float LeftHandCollTime { get; set; } = 1f;
        public float RightHandCollTime { get; set; } = 1f;

        [Header("일반 스테이터스: 실시간으로 변하는 수치")]
        public float Health { get; set; }
        public float Mana { get; set; }

        void Awake()
        {
            Init();
            OnDeadAction += Death;
        }

        public void Init()
        {
            Health = MaxHealth;
            Mana = MaxMana;
        }

        #region 체력 관련
        public void TakeDamage(float damage)
        {
            if (IsDead || PlayerManager.Instance.PlayerDash.IsDash)
                return;

            UI_InGameEventBus.OnShowBloodCanvas?.Invoke();
            ShowDamageText(damage);
            UpdateHealth(-damage);

            if (Health <= 0)
            {
                OnDeadAction.Invoke();
            }
        }

        // 체력 회복
        public void RecoverHealth(float amount) => UpdateHealth(amount);

        // 체력 갱신
        void UpdateHealth(float delta)
        {
            Health += delta;
            Health = Mathf.Clamp(Health, 0, MaxHealth);
            UI_InGameEventBus.OnPlayerHpSliderValueUpdate?.Invoke(Health);
        }
        #endregion

        #region 마나 관련
        // 마나 재생
        public void RegenerateMana(float amount) => UpdateMana(amount);

        // 마나 회복
        public void RecoveryMana(float amount) => UpdateMana(amount);

        // 마나 소비
        public bool SpendMana(float amount)
        {
            if (Mana < amount)
                return false;

            UpdateMana(-amount);
            return true;
        }


        // 마나 갱신
        void UpdateMana(float delta)
        {
            Mana += delta;
            Mana = Mathf.Clamp(Mana, 0, MaxMana);
            UI_InGameEventBus.OnPlayerMpSliderValueUpdate?.Invoke(Mana);
        }
        #endregion

        // 데미지 텍스트 띄우기
        void ShowDamageText(float damage)
        {
            TextMeshPro damageText = Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
            damageText.transform.position = transform.position;
            damageText.text = damage.ToString();
        }

        // 사망
        void Death()
        {
            IsDead = true;
            Destroy(gameObject);
        }
    }
}