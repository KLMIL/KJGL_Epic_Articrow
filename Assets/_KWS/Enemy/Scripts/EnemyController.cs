using BMC;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Game.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Enemy Status")]
        [SerializeField] EnemyStatusSO _statusOrigin;
        public EnemyStatusSO StatusOrigin => _statusOrigin;

        [HideInInspector] public EnemyStatusSO Status;

        [SerializeField] public string _currentState;
        

        [Header("Components")]
        public SpriteRenderer SpriteRenderer;
        [HideInInspector] public EnemyFSMCore FSM;
        EnemyAnimation _animation;
        EnemyMovement _movement;
        EnemyDealDamage _dealDamage;
        Rigidbody2D _rb;
        [HideInInspector] public EnemyAttackIndicator AttackIndicator;


        [Header("FSM")]
        public List<EnemyBehaviourUnit> Behaviours = new();       // 직접 할당할 FSM 상태 리스트
        public Dictionary<string, float> lastAttackTimes = new(); // 공격 쿨타임 저장 딕셔너리


        [Header("Others")]
        public GameObject SpawnEffectPrefab;

        [HideInInspector] public Transform Player;
        [HideInInspector] public Transform Attacker = null;

        public event Action OnDeath;



        #region Properties
        public Coroutine IndicatorCoroutine { get; set; }
        public string CurrentStateName => FSM.CurrentStateName;
        public string CurrentAnimation => _animation.CurrentAnimation;
        #endregion


        #region Initialization
        private void Awake()
        {
            // 스텟 SO를 복사
            Status = Instantiate(_statusOrigin);

            // 할당된 Behaviour 중 Attack 분류에 속하면 쿨타임 딕셔너리에 추가
            foreach (var behaviour in Behaviours)
            {
                if (behaviour.action is MeleeAttackActionSO ||
                    behaviour.action is ProjectileAttackActionSO ||
                    behaviour.action is SpecialAttackActionSO ||
                    behaviour.action is DecidePatternActionSO)
                {
                    string key = behaviour.stateName;
                    if (!lastAttackTimes.ContainsKey(key))
                    {
                        lastAttackTimes[key] = -Mathf.Infinity;
                    }
                }
            }

            // FSM 생성
            FSM = new EnemyFSMCore(this, Behaviours);
            AttackIndicator = GetComponentInChildren<EnemyAttackIndicator>(true);
            if (AttackIndicator == null)
            {
                Debug.Log($"{gameObject.name}: No AttackIndicator assigned");
            }

            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            StageManager.Instance.CurrentRoom.GetComponent<NormalRoom>().EnrollEnemy(this);
        }

        private void Start()
        {
            _animation = GetComponentInChildren<EnemyAnimation>();
            SpriteRenderer = _animation.SpriteRenderer;
            
            _movement = GetComponent<EnemyMovement>();
            _movement.Init(this, SpriteRenderer, _rb);
            _dealDamage = GetComponent<EnemyDealDamage>();

            Player = GameObject.FindWithTag("Player")?.transform;

            SetAllChildrenActive(false);
        }
        #endregion


        private void Update()
        {
            if (Player == null)
            {
                Player = GameObject.FindWithTag("Player")?.transform;
                if (Player == null) return;
            }

            FSM.Update();
        }

        #region Public API
        public void PlayAnimationOnce(string animName) => _animation.PlayAnimationOnce(animName);
        public void ForceToNextState() => FSM.ForceToNextState();
        public void DealDamageToPlayer(float damage, Transform targetTransform, bool forceToNextState = false) => _dealDamage.DealDamageToPlayer(damage, targetTransform, forceToNextState);
        public void MoveTo(Vector3 dir, float duration, string moveType, bool inverse = false) => _movement.MoveTo(dir, duration, moveType, inverse);
        public void StopMove() => _movement.Stop();
        public void StartKnockbackCoroutine(Vector2 direction, float distance, float duration, int steps)
        {
            StartCoroutine(_movement.StepKnockback(direction, distance, duration, steps));
        }
        public bool IsPatternReady(string patternName, float cooldown)
        {
            float lastUsed = 0f;
            if (lastAttackTimes.TryGetValue(patternName, out var v))
            {
                lastUsed = v;
            }
            return (Time.time - lastUsed) >= cooldown;
        }
        public void ChangeMoveSpeedMultiply(float multiplier)
        {
            _movement.moveSpeedMultiply = multiplier;
        }
        public void OnEnemyDieAction() => OnDeath?.Invoke();
        public void MakeSpawnEffect()
        {
            if (SpawnEffectPrefab != null)
            {
                Instantiate(SpawnEffectPrefab, transform.position, Quaternion.identity);
            }
            
        }
        public void SetAllChildrenActive(bool isActive)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(isActive);
            }
        }
        #endregion
    }
}