using System.Collections;
using Unity.Behavior;
using UnityEngine;
using YSJ;

namespace BMC
{
    public class GolemBossStatus : BossStatus, IDamagable
    {
        GolemFSM _golemFSM;
        Rigidbody2D _rb;
        CircleCollider2D _collider;
        SpriteRenderer _core;
        Coroutine _takeDamageCoroutine;

        void Awake()
        {
            _golemFSM = GetComponent<GolemFSM>();
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CircleCollider2D>();
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            _visual = GetComponentInChildren<SpriteRenderer>();
            _core = _visual.GetComponentInChildren<SpriteRenderer>();
            _showDamageText = GetComponentInChildren<ShowDamageText>();
            Init();
        }

        public override void Init()
        {
            Health = 3000;
            Damage = 1f;
        }

        void Update()
        {
            TestCode();
        }

        public void TakeDamage(float damage, Transform attacker = null)
        {
            if (IsDead)
                return;

            Health -= damage;

            _showDamageText.Show(damage);

            //Debug.LogError("데미지 받아서 색변경됨");

            if(_takeDamageCoroutine == null)
                _takeDamageCoroutine = StartCoroutine(TakeDamageColor());
            
            // 보스 체력 UI
            UI_InGameEventBus.OnBossHpSliderValueUpdate?.Invoke(Health);

            // 사망
            if (Health <= 0)
            {
                Die();
            }
        }

        // 피격 시, 색상 변경
        IEnumerator TakeDamageColor()
        {
            _visual.color = Color.gray;
            _core.color = Color.gray;
            //Debug.LogError("피격 색상 변경");
            yield return _colorChangeTime;
            //Debug.LogError("피격 색상 복구");
            _visual.color = Color.white;
            _core.color = Color.white;
            _takeDamageCoroutine = null;
        }

        public override void Die()
        {
            IsDead = true;
            _rb.linearVelocity = Vector2.zero; // 움직임 정지
            _collider.enabled = false;
            _behaviorGraphAgent.SetVariableValue("IsDead", IsDead);
            _behaviorGraphAgent.SetVariableValue("CurrentState", GolemBossState.Die);
            UI_InGameEventBus.OnToggleBossHpSlider?.Invoke(false); // 보스 HP 슬라이더 숨김
            _golemFSM.DisableAttack();                             // 공격 인디케이터 비활성화
            Managers.Sound.PlaySFX(Define.SFX.GolemDie);

            // 피격 색상 변경 중지
            StopAllCoroutines();
            _visual.color = Color.gray;

            //SteamAchievement.instance.Achieve(SteamAchievement.AchievementType.GolemBossClear);
        }

        // GolemBossDie 애니메이션 끝에서 실행할 애니메이션 이벤트
        public void OnDieEndAniationEvent()
        {
            Debug.Log("보스 방 클리어");
            StageManager.Instance.CurrentRoom.RoomClearComplete();
        }

        #region 테스트 코드
        public void TestCode()
        {
            if (Input.GetKeyDown(KeyCode.F9))
            {
                TakeDamage(1000f);
            }
        }
        #endregion
    }
}