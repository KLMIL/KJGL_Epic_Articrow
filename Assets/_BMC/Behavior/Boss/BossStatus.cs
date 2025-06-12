using TMPro;
using Unity.Behavior;
using UnityEngine;
using YSJ;

namespace BMC
{
    public class BossStatus : MonoBehaviour, IDamagable
    {
        Animator _anim;
        Animator Anim => _anim;

        CapsuleCollider2D _collider;

        BehaviorGraphAgent _behaviorGraphAgent;
        TextMeshPro DamageTextPrefab;

        [Header("상태")]
        [field: SerializeField] public bool IsDead { get; set; }

        [Header("일반 스테이터스")]
        [field: SerializeField] public float Health { get; set; }

        void Awake()
        {
            _anim = GetComponent<Animator>();
            _collider = GetComponent<CapsuleCollider2D>();
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            Init();

            DamageTextPrefab = Managers.Resource.DamageText;
        }

        public void Init()
        {
            Health = 5000;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
                TakeDamage(5);
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

            TextMeshPro spawnedObj = Instantiate(DamageTextPrefab, transform.position, Quaternion.identity);
            spawnedObj.text = damage.ToString();

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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Rigidbody2D>()?.AddForce((collision.transform.position - transform.position).normalized * 10f, ForceMode2D.Impulse);
                collision.gameObject.GetComponent<IDamagable>()?.TakeDamage(5f);
            }
        }
    }
}