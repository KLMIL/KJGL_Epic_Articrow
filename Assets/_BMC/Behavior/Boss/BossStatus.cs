using System.Collections;
using TMPro;
using Unity.Behavior;
using UnityEngine;
using YSJ;

namespace BMC
{
    public class BossStatus : MonoBehaviour, IDamagable
    {
        CapsuleCollider2D _collider;
        WaitForSeconds _colorChangeTime = new WaitForSeconds(0.25f);
        BehaviorGraphAgent _behaviorGraphAgent;
        TextMeshPro _damageText;

        [Header("상태")]
        [field: SerializeField] public bool IsDead { get; set; }

        [Header("일반 스테이터스")]
        [field: SerializeField] public float Health { get; set; }
        [field: SerializeField] public float Damage { get; set; } = 10f;

        Coroutine _hitEffectCoroutine;
        SpriteRenderer _visual;

        void Awake()
        {
            _collider = GetComponent<CapsuleCollider2D>();
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            _visual = GetComponentInChildren<SpriteRenderer>();
            Init();
        }

        public void Init()
        {
            Health = 5000;
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.G))
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
            _damageText.transform.position = this.transform.position + this.transform.up * 1.5f;

            StartCoroutine(TakeDamageColor());
            // 보스 체력 UI
            UI_InGameEventBus.OnBossHpSliderValueUpdate?.Invoke(Health);
            if (Health <= 0)
            {
                // 피격 색상 변경 중지
                StopAllCoroutines();
                _visual.color = Color.gray;

                IsDead = true;
                _collider.enabled = false;
                _behaviorGraphAgent.SetVariableValue("IsDead", IsDead);
                _behaviorGraphAgent.SetVariableValue("CurrentState", BossState.Die);
            }
        }

        // 피격 색상 변경
        IEnumerator TakeDamageColor()
        {
            _visual.color = Color.gray;
            yield return _colorChangeTime;
            _visual.color = Color.white;
        }
    }
}