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
        PlayerAnimator _playerAnimator;
        Rigidbody2D _rb;

        [Header("피격")]
        SpriteRenderer _spriteRenderer;
        TextMeshPro _damageText;
        public bool IsHurt { get; private set; } = false; // 피격 여부
        float _cameraShakeIntensity = 0.5f;
        float _cameraShakeTime = 0.25f;
        float _invincibleTime = 1f;
        int _colorChangeLoopCount = 20; // 색상 변경 루프 횟수

        [Header("사망")]
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
                _health = Mathf.Clamp(_health, 0, MaxHealth);
                UI_InGameEventBus.OnPlayerHpSliderValueUpdate?.Invoke(_health);
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
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _playerAnimator = GetComponent<PlayerAnimator>();
            OnDeadAction += Die;
            Init();
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
            // 마나 소비량 줄여주는 오프셋 적용
            float spendAmount = Mathf.Max(amount - _spendManaOffsetAmount, 0f);

            if (Mana < spendAmount)
                return false;

            Mana += -spendAmount;
            //UpdateMana(-amount);
            return true;
        }
        #endregion

        #region 피해 및 사망 관련
        // 피해 받기
        public void TakeDamage(float damage)
        {
            if (IsDead || IsHurt || PlayerManager.Instance.PlayerDash.IsDash)
            {
                return;
            }

            if (!IsHurt)
            {
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
                Health -= damage;
                StartCoroutine(InvincibleCoroutine(_invincibleTime));
                GameManager.Instance.CameraController.ShakeCamera(_cameraShakeIntensity, _cameraShakeTime);
                _playerAnimator.CurrentState |= PlayerAnimator.State.Hurt;
            }

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
                _damageText.text = damage.ToString("F0");
            }
            _damageText.transform.position = this.transform.position + this.transform.up;
        }

        // 사망
        void Die()
        {
            IsDead = true;

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