using System.Collections;
using TMPro;
using Unity.Behavior;
using UnityEngine;
using YSJ;

namespace BMC
{
    public class BossStatus : MonoBehaviour, IDamagable
    {
        Animator _anim;
        CapsuleCollider2D _collider;
        BehaviorGraphAgent _behaviorGraphAgent;

        [Header("상태")]
        [field: SerializeField] public bool IsDead { get; set; }

        [Header("일반 스테이터스")]
        [field: SerializeField] public float Health { get; set; }
        [field: SerializeField] public float Damage { get; set; } = 10f;

        Coroutine _hitEffectCoroutine;
        SpriteRenderer _visual;

        void Awake()
        {
            _anim = GetComponent<Animator>();
            _collider = GetComponent<CapsuleCollider2D>();
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            _visual = GetComponentInChildren<SpriteRenderer>();
            Init();

        }

        public void Init()
        {
            Health = 5000;
        }

        public void TakeDamage(float damage)
        {
            Debug.Log("보스 처맞음");

            if (IsDead)
                return;

            // 애니메이션 재생
            AnimatorStateInfo currentStateInfo = _anim.GetCurrentAnimatorStateInfo(0);
            if(!currentStateInfo.IsName("Shoot") && !currentStateInfo.IsName("Rush"))
            {
                if(!currentStateInfo.IsName("Hit") || currentStateInfo.IsName("Idle"))
                {
                    _anim.Play("Hit");
                }
            }

            Health -= damage;

            Debug.Log($"보스 체력: {Health}");

            TextMeshPro damageText = Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
            damageText.text = damage.ToString();
            damageText.transform.position = transform.position;

            // 보스 체력 UI
            UI_InGameEventBus.OnBossHpSliderValueUpdate?.Invoke(Health);
            if (Health <= 0)
            {
                IsDead = true;
                _collider.enabled = false;
                _behaviorGraphAgent.SetVariableValue("IsDead", IsDead);
                _behaviorGraphAgent.SetVariableValue("CurrentState", BossState.Die);
            }
        }

        // 피격 이펙트 코루틴
        IEnumerator HitEffectCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Rigidbody2D>()?.AddForce((collision.transform.position - transform.position).normalized * 10f, ForceMode2D.Impulse);
                collision.gameObject.GetComponent<IDamagable>()?.TakeDamage(Damage);
            }
        }
    }
}