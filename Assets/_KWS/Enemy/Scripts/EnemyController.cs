using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EnemyDealDamage))]
    [RequireComponent(typeof(EnemyTakeDamage))]
    [RequireComponent(typeof(EnemyAnimation))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Enemy Status")]
        [SerializeField] EnemyStatusSO _statusOrigin;
        [HideInInspector] public EnemyStatusSO Status;

        [Header("Components")]
        public SpriteRenderer SpriteRenderer;
        [HideInInspector] public EnemyFSMCore FSM;
        EnemyAnimation _animation;
        EnemyMovement _movement;
        EnemyDealDamage _dealDamage;

        [Header("FSM")]
        public List<EnemyBehaviourUnit> Behaviours = new();       // 직접 할당할 FSM 상태 리스트
        public Dictionary<string, float> lastAttackTimes = new(); // 공격 쿨타임 저장 딕셔너리

        [HideInInspector] public Transform Player;
        [HideInInspector] public Transform Attacker = null;

        Coroutine markingCoroutine;


        public string CurrentStateName => FSM.CurrentStateName;
        public string CurrentAnimation => _animation.CurrentAnimation;




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
                    behaviour.action is SpecialAttackActionSO)
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
        }

        private void OnEnable()
        {
            //SpawnTime = Time.time;
            //FSM.ChangeState("Spawned");
        }

        private void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            _animation = GetComponent<EnemyAnimation>();
            _movement = GetComponent<EnemyMovement>();
            _dealDamage = GetComponent<EnemyDealDamage>();
            Player = GameObject.FindWithTag("Player")?.transform;
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

        public void StartMarkingCoroutine(float multiply, float duration)
        {
            if (markingCoroutine != null)
            {
                StopCoroutine(markingCoroutine);
            }
            
            // TODO: 덮어쓰는 방식을 어떻게 할지 결정할 것
            FSM.enemyDamagedMultiply = multiply;
            FSM.enemyDamagedMultiplyRemainTime = Time.time + duration;

            Debug.LogError($"Time do? : {FSM.enemyDamagedMultiply < Time.time}");

            markingCoroutine = StartCoroutine(MarkingRestoreCoroutine(duration));
        }
        #endregion


        private IEnumerator MarkingRestoreCoroutine(float duration)
        {
            yield return new WaitForSeconds(duration);
            markingCoroutine = null;
            FSM.enemyDamagedMultiply = 1f;
        }
    }
}