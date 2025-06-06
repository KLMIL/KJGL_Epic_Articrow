using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Core Field
    // 스텟 정보
    [Header("Status")]
    [SerializeField] EnemyStatusSO _statusOrigin;
    [HideInInspector] public EnemyStatusSO Status;

    [HideInInspector] public Transform Player;
    EnemyAnimation Animation;


    // 해당 유닛이 수행할 행동 배열
    [Header("Behaviour List")]
    public List<EnemyBehaviourUnit> Behaviours = new();


    // 공격 쿨타임 딕셔너리
    [HideInInspector]
    public Dictionary<string, float> lastAttackTimes = new Dictionary<string, float>();


    // 현재 FSM 상태, 현재 애니메이션
    [HideInInspector] public string CurrentStateName = "Idle";
    [HideInInspector] public string CurrentAnimation = "";

    #endregion

    #region Behaviour Field 
    // ----- 조건, 상태 관련 변수 -----
    // 몬스터 피격 처리
    [HideInInspector] public bool isDamaged = false;
    [HideInInspector] public float pendingDamage = 0;

    // 공격, 투사체 소환 등
    [HideInInspector] public int projectileFiredCount = 0;
    [HideInInspector] public float projectileIntervalTimer = 0;
    [HideInInspector] public bool isSpawnedMite = false;

    [HideInInspector] public bool isRushing = false;
    [HideInInspector] public Vector3 rushDirection = Vector3.zero;

    [HideInInspector] public bool isContactDamageActive = false;
    [HideInInspector] public float currentActionDamageMultiply = 1.0f;

    // 이동
    [HideInInspector] public Vector3 randomMoveDirection = Vector3.zero;
    [HideInInspector] public float randomMoveChangeCooldown = 0f;
    #endregion


    #region Initialization
    private void Awake()
    {
        Status = Instantiate(_statusOrigin);

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
    }

    private void Start()
    {
        Animation = GetComponent<EnemyAnimation>();
        // Null체크 해야함.
        Player = GameObject.FindWithTag("Player")?.transform;
    }
    #endregion


    // 추후, 공격 중 피격상태 등 커버를 위해 병렬 FSM 구현해야함.
    // 현재 상태에서는, 0번에 Idle, None->Soft->Hard 순서로 할당.
    private void Update()
    {
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player")?.transform;
            if (Player == null) return;
        }
        if (!IsBehaviourAssigned()) return;
        if (IsEnemyDie()) return;

        // 현재 상태 가져오기
        int idx = Behaviours.FindIndex(b => b.stateName == CurrentStateName);
        EnemyBehaviourUnit current = Behaviours[idx];

        // 인터럽트 흐름에 따라 수행
        if (HandleHardInterrupt(current)) return;
        HandleSoftInterrupt(current);
        HandleNoneInterrupt(current);
    }

    #region State Check
    private bool IsBehaviourAssigned()
    {
        // 0. 행동 미할당 예외 처리. 추후 안정화 작업 후 삭제 예정
        if (Behaviours == null || Behaviours.Count == 0)
        {
            Debug.LogWarning("No behaviour assigned");
            return false;
        }
        return true;
    }

    private bool IsEnemyDie()
    {
        // Die State
        if (CurrentStateName == "Die")
        {
            return true;
        }
        return false;
    }

    #endregion

    #region Interrupt
    private bool HandleHardInterrupt(EnemyBehaviourUnit current)
    {
        // 0. 현재 조건이 Hard일 경우, 상태가 전이되기 전까지 계속해서 수행
        if (current.interruptType == InterruptType.Hard)
        {
            HandleNoneInterrupt(current);
            return true; // Hard Type Interrupt 수행 중에는 이후 검사 수행하지 않음
        }


        // 1. Hard: 강제 인터럽트 조건 검사
        // ex) 죽음, 특수공격 등
        int hardIdx = Behaviours.FindIndex(
                b => b.interruptType == InterruptType.Hard &&
                     b.condition.IsMet(this));

        if (hardIdx >= 0)
        {
            ChangeState(Behaviours[hardIdx].stateName);
        }
        return false;
    }

    private void HandleSoftInterrupt(EnemyBehaviourUnit current)
    {
        // 2. Soft: 우선순위 인터럽트 순서대로 검사
        // ex) 공격, 소환 등 => 특정 상황에서는 hard 일 수도 있음. 
        int softIdx = -1;
        if (current.interruptType != InterruptType.Soft)
        {
            softIdx = Behaviours.FindIndex(
                    b => b.interruptType == InterruptType.Soft &&
                         b.condition.IsMet(this));
        }

        if (softIdx >= 0)
        {
            ChangeState(Behaviours[softIdx].stateName);
        }
    }

    private void HandleNoneInterrupt(EnemyBehaviourUnit current)
    {
        // 3. None: 기본 루프 수행
        // Idle, RandomMove 등
        current.elapsedTime += Time.deltaTime;
        if (current.elapsedTime <= current.duration)
        {
            Debug.Log($"Act: {current.stateName}");
            current.action?.Act(this);
        }
        else
        {
            ChangeState(current.nextStateName);
        }
    }

    #endregion

    #region Play Animations
    // 애니메이션 중복 재생 방지용 함수
    private void PlayAnimationOnce(string animName)
    {
        if (CurrentAnimation == animName) return;
        Animation.Play(animName);
        CurrentAnimation = animName;
    }
    #endregion

    #region State Change
    private void ForceToNextState()
    {
        int idx = Behaviours.FindIndex(b => b.stateName == CurrentStateName);

        if (idx >= 0)
        {
            string next = Behaviours[idx].nextStateName;
            ChangeState(next);
        }
    }

    // 일반 상태 전이 함수
    private void ChangeState(string nextStateName)
    {
        // 같은 상태로의 전이는 아무것도 하지 않음
        if (CurrentStateName == nextStateName) return; 


        int prevIdx = Behaviours.FindIndex(b => b.stateName == CurrentStateName);
        int nextIdx = Behaviours.FindIndex(b => b.stateName == nextStateName);

        // 이전 상태 Exit
        if (prevIdx >= 0)
        {
            Behaviours[prevIdx].action?.OnExit(this);
        }

        // 상태 이름 변경
        CurrentStateName = nextStateName;

        // 다음 상태 Enter
        if (nextIdx >= 0)
        {
            Behaviours[nextIdx].ResetTimer();
            Behaviours[nextIdx].action?.OnEnter(this);
            PlayAnimationOnce(Behaviours[nextIdx].animationName);
        }
    }
    #endregion

    #region Attack Check
    public void DealDamageToPlayer(float damage, bool forceToNextState = false)
    {
        IDamagable target = Player.GetComponent<IDamagable>();
        if (target != null)
        {
            target.TakeDamage(damage);

            if (forceToNextState)
            {
                ForceToNextState();
            }
        }
    }
    #endregion
}
