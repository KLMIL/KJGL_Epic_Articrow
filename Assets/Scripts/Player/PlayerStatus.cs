using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BMC;

namespace YSJ
{
    public class PlayerStatus : MonoBehaviour, IDamagable
    {
        TextMeshPro _damageText;
        
        SpriteRenderer _spriteRenderer;
        WaitForSeconds _colorChangeTime = new WaitForSeconds(0.25f);

        public static Action OnDeadAction;

        public bool IsDead { get; private set; } = false;

        #region 기준 스테이터스
        [Header("기준 스테이터스")]
        float _maxHealth = 100f;        // 최대 체력
        float _maxMana = 100f;          // 최대 마나
        float _minRightCoolTime = 0.1f; // 오른손 최소 쿨타임
        float _minRightDamage = 10f;    // 최소 데미지

        public float MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
            }
        }

        public float MaxMana
        {
            get => _maxMana;
            set
            {
                _maxMana = value;
                UI_InGameEventBus.OnPlayerMpSliderMaxValueUpdate?.Invoke(_maxMana);
                UI_InGameEventBus.OnPlayerMpSliderValueUpdate?.Invoke(Mana);
            }
        }
        #endregion

        #region 스테이터스
        [Header("스테이터스")]
        float _health;                      // 체력
        float _mana;                        // 마나
        float _rightCoolTime = 0f;          // 오른손 추가 쿨타임
        float _rightDamage = 0f;           // 오른손 추가 데미지
        float _spendManaOffsetAmount = 0f;  // 마나 소모량 감소

        public float Health
        {
            get => _health;
            set
            {
                _health = value;
                //_health = Mathf.Clamp(_health, 0, MaxHealth);
                //UI_InGameEventBus.OnPlayerHpSliderValueUpdate?.Invoke(_health);
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
                UI_InGameEventBus.OnPlayerMpSliderValueUpdate?.Invoke(_mana);
            }
        }

        public float RightCoolTime
        {
            get => _rightCoolTime;
            set
            {
                _rightCoolTime = value;
                _rightCoolTime = Mathf.Clamp(_rightCoolTime, _minRightCoolTime, _rightCoolTime);
            }
        }

        public float RightDamage
        {
            get => _rightDamage;
            set
            {
                _rightDamage = value;
                _rightDamage = Mathf.Clamp(_rightDamage, _minRightDamage, _rightDamage);
            }
        }

        public float SpendManaOffsetAmount
        {
            get => _spendManaOffsetAmount;
            set
            {
                _spendManaOffsetAmount = value;
                _spendManaOffsetAmount = Mathf.Clamp(_spendManaOffsetAmount, 0f, float.MaxValue);
            }
        }
        #endregion

        void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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

            //YSJ : 베리어가 있는 지 확인
            List<Collider2D> colliders = new();
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask("Player"));
            filter.useTriggers = true;
            Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, colliders);
            foreach (Collider2D col in colliders)
            {
                if (col.TryGetComponent<Barrier>(out Barrier barrier)) 
                {
                    barrier.TakeDamage(damage); // 베리어가 있으면 베리어가 대신받음
                    return;
                }
            }

            UI_InGameEventBus.OnShowBloodCanvas?.Invoke();
            ShowDamageText(damage);
            UpdateHealth(-damage);
            StartCoroutine(TakeDamageColor());

            // YSJ : 데미지 받으면 피격 애니메이션 재생
            PlayerAnimator playerAnimator = GetComponent<PlayerAnimator>();
            if (playerAnimator)
            {
                playerAnimator.CurrentState |= PlayerAnimator.State.Hurt;
            }

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
        public void RegenerateMana(float amount) => Mana += amount;

        // 마나 회복
        public void RecoveryMana(float amount) => Mana += amount;

        // 마나 소비
        public bool SpendMana(float amount)
        {
            // 마나 소비량 줄여주는 오프셋 적용
            float spendAmount = Mathf.Max(amount - _spendManaOffsetAmount, 0f);

            if (Mana < spendAmount)
                return false;

            Mana += -spendAmount;
            //UpdateMana(-amount);
            return true;
        }


        // 마나 갱신
        void UpdateMana(float delta)
        {
            Mana += delta;
            //Mana = Mathf.Clamp(Mana, 0, MaxMana);
            //UI_InGameEventBus.OnPlayerMpSliderValueUpdate?.Invoke(Mana);
        }
        #endregion

        // 데미지 텍스트 띄우기
        void ShowDamageText(float damage)
        {
            /*TextMeshPro damageText = Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
            damageText.transform.position = transform.position;
            damageText.text = damage.ToString();*/

            // 대미지 부여 텍스트
            if (_damageText != null && _damageText.gameObject.activeInHierarchy)
            {
                _damageText.text = (float.Parse(_damageText.text) + damage).ToString();

                Color color = _damageText.color;
                color.a = 1;
                _damageText.color = color;
            }
            else
            {
                _damageText = Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
                _damageText.text = damage.ToString();
            }
            _damageText.transform.position = this.transform.position + this.transform.up;
        }

        // 피격 색상 변경
        IEnumerator TakeDamageColor()
        {
            _spriteRenderer.color = Color.gray;
            yield return _colorChangeTime;
            _spriteRenderer.color = Color.white;
        }

        // 사망
        void Death()
        {
            IsDead = true;

            // 피격 색상 변경 중지
            StopAllCoroutines();
            _spriteRenderer.color = Color.gray;

            // 물리 효과 적용 x
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        void OnDestroy()
        {
            OnDeadAction = null;
        }
    }
}