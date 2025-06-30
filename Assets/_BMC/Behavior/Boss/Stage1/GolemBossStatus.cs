using System.Collections;
using Unity.Behavior;
using UnityEngine;

namespace BMC
{
    public class GolemBossStatus : BossStatus, IDamagable
    {
        CapsuleCollider2D _collider;
        SpriteRenderer _core;

        void Awake()
        {
            _collider = GetComponent<CapsuleCollider2D>();
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            _visual = GetComponentInChildren<SpriteRenderer>();
            _core = _visual.GetComponentInChildren<SpriteRenderer>();
            Init();
        }

        public override void Init()
        {
            Health = 5000;
            Damage = 10f;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                TakeDamage(1000f);
            }
        }

        public void TakeDamage(float damage)
        {
            if (IsDead)
                return;

            Health -= damage;

            Debug.Log($"보스 체력: {Health}");
            ShowTakeDamageText(damage);

            Debug.LogError("데미지 받아서 색변경됨");
            StartCoroutine(TakeDamageColor());
            
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
        }

        public override void Die()
        {
            IsDead = true;
            _collider.enabled = false;
            _behaviorGraphAgent.SetVariableValue("IsDead", IsDead);
            _behaviorGraphAgent.SetVariableValue("CurrentState", GolemBossState.Die);

            // 피격 색상 변경 중지
            StopAllCoroutines();
            _visual.color = Color.gray;
        }

        // GolemBossDie 애니메이션 끝에서 실행할 애니메이션 이벤트
        public void OnDieEndAniationEvent()
        {
            Debug.Log("보스 방 클리어");
            StageManager.Instance.CurrentRoom.RoomClearComplete();
        }
    }
}