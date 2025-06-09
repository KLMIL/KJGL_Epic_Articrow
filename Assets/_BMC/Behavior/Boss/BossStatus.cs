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

        BehaviorGraphAgent _behaviorGraphAgent;
        TextMeshPro DamageTextPrefab;

        [Header("상태")]
        [field: SerializeField] public bool IsDead { get; set; }

        [Header("일반 스테이터스")]
        [field: SerializeField] public float Health { get; set; }

        void Awake()
        {
            _anim = GetComponent<Animator>();
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            Init();
            DamageTextPrefab = Managers.Resource.Load<TextMeshPro>("Text/DamageText");
        }

        public void Init()
        {
            Health = 100;
        }

        public void TakeDamage(float damage)
        {
            if (IsDead)
                return;

            // 애니메이션 재생
            AnimatorStateInfo currentStateInfo = _anim.GetCurrentAnimatorStateInfo(0);
            if (!currentStateInfo.IsName("Hit"))
                _anim.Play("Hit");

            Health -= damage;
            TextMeshPro spawnedObj = Instantiate(DamageTextPrefab, transform.position, Quaternion.identity);
            spawnedObj.text = damage.ToString();
            UI_InGameEventBus.OnHpSliderValueUpdate?.Invoke(Health);
            if (Health <= 0)
            {
                IsDead = true;
                _behaviorGraphAgent.SetVariableValue("IsDead", IsDead);
                _behaviorGraphAgent.SetVariableValue("CurrentState", BossState.Die);
            }
        }
    }
}