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
        [HideInInspector] public EnemyStatusSO Status;

        [SerializeField] public string _currentState;

        public EnemyStatusSO StatusOrigin => _statusOrigin;

        [Header("Components")]
        public SpriteRenderer SpriteRenderer;
        [HideInInspector] public EnemyFSMCore FSM;
        EnemyAnimation _animation;
        EnemyMovement _movement;
        EnemyDealDamage _dealDamage;
        Rigidbody2D _rb;

        [Header("FSM")]
        public List<EnemyBehaviourUnit> Behaviours = new();       // 직접 할당할 FSM 상태 리스트
        public Dictionary<string, float> lastAttackTimes = new(); // 공격 쿨타임 저장 딕셔너리

        [HideInInspector] public Transform Player;
        [HideInInspector] public Transform Attacker = null;

        [HideInInspector] public EnemyAttackIndicator AttackIndicator;

        Coroutine markingCoroutine;
        Coroutine gravitySurgeCoroutine;
        Coroutine indicatorCoroutine;

        [Header("Others")]
        public GameObject SpawnEffectPrefab;

        public string CurrentStateName => FSM.CurrentStateName;
        public string CurrentAnimation => _animation.CurrentAnimation;

        public event Action OnDeath;


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
            AttackIndicator = GetComponentInChildren<EnemyAttackIndicator>(true);
            if (AttackIndicator == null)
            {
                Debug.Log($"{gameObject.name}: No AttackIndicator assigned");
            }

            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            //SpawnTime = Time.time;
            //FSM.ChangeState("Spawned");
            StageManager.Instance.CurrentRoom.GetComponent<NormalRoom>().EnrollEnemy(this);
        }

        private void Start()
        {
            //SpriteRenderer = GetComponent<SpriteRenderer>();
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

        // 임시로 피격 모션을 보여주기 위한 함수들
        public void StartTintCoroutine(Color color, float duration)
        {
            StartCoroutine(SpriteTintRepeatCoroutine(color, duration));
        }

        public void StartTintCoroutineOnce(Color color)
        {
            StartCoroutine(SpriteTintOnceCoroutine(color));
        }

        public void StartGravitySurgeCoroutine(Coroutine coroutine)
        {
            gravitySurgeCoroutine = coroutine;
        }

        public void EndGravitySurgeCoroutine()
        {
            gravitySurgeCoroutine = null;
        }

        public bool IsGravitySurgeCoroutineGO()
        {
            return gravitySurgeCoroutine != null;
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

        public void StartIndicatorCoroutine(Coroutine coroutine)
        {
            indicatorCoroutine = coroutine;
        }

        public void EndIndicatorCoroutine()
        {
            indicatorCoroutine = null;
        }

        public bool IsIndicatorCoroutineGo()
        {
            return indicatorCoroutine != null;
        }

        public Coroutine GetIndicatorCoroutine()
        {
            return indicatorCoroutine;
        }

        public void SetAllChildrenActive(bool isActive)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(isActive);
            }
        }
        #endregion


        private IEnumerator MarkingRestoreCoroutine(float duration)
        {
            yield return new WaitForSeconds(duration);
            markingCoroutine = null;
            FSM.enemyDamagedMultiply = 1f;
        }

        private IEnumerator SpriteTintRepeatCoroutine(Color color, float duration)
        {
            Debug.LogError("Called Tint Coroutine");

            float elapsed = 0f;
            bool currColor = true;
            SpriteRenderer.color = color;

            while (elapsed < duration)
            {
                yield return new WaitForSeconds(0.2f);
                elapsed += 0.2f;
                if (currColor)
                {
                    SpriteRenderer.color = Color.white;
                    currColor = !currColor;
                }
                else
                {
                    SpriteRenderer.color = color;
                    currColor = !currColor;
                }
            }

            SpriteRenderer.color = Color.white;
        }

        private IEnumerator SpriteTintOnceCoroutine(Color color)
        {
            SpriteRenderer.color = color;
            yield return new WaitForSeconds(0.5f);
            SpriteRenderer.color = Color.white;
        }
    }
}