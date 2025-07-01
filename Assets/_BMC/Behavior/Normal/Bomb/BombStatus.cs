using System.Collections;
using Unity.Behavior;
using UnityEngine;

namespace BMC
{
    public class BombStatus : EnemyStatus, IDamagable
    {
        CircleCollider2D _collider;

        void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            _visual = GetComponentInChildren<SpriteRenderer>();
        }

        public override void Init()
        {
            Health = 5;
            Damage = 10f;
        }

        public void TakeDamage(float damage)
        {
            if (IsDead)
                return;

            Health -= damage;

            ShowTakeDamageText(damage);
            StartCoroutine(TakeDamageColor());

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
            //Debug.LogError("피격 색상 변경");
            yield return _colorChangeTime;
            //Debug.LogError("피격 색상 복구");
            _visual.color = Color.white;
        }

        public override void Die()
        {
            IsDead = true;
            _collider.enabled = false;
            _behaviorGraphAgent.SetVariableValue("IsDead", IsDead);
            //_behaviorGraphAgent.SetVariableValue("CurrentState", BombState.Explosion);

            // 피격 색상 변경 중지
            StopAllCoroutines();
        }

        // BombExplosion 애니메이션 끝에서 실행할 애니메이션 이벤트
        public void OnExplosionEndAniationEvent()
        {
        }
    }
}