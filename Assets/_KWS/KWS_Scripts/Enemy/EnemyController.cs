using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using YSJ;

public class EnemyController : MonoBehaviour
{
    [Header("Status: ScriptableObject")]
    public EnemyStatusSO StatusOrigin;
    public EnemyStatusSO Status;

    [Header("Player: Find by Tag")]
    public Transform Player;
    [Header("Animation Handler: GetComponent")]
    public EnemyAnimation Animation;

    [Space(10)]
    [Header("Behaviour List Assign on Inspector")]
    public List<EnemyBehaviourUnit> Behaviours = new();

    string _currentStateName = "Idle";
    string _currentAnimation = "";

    public string CurrentStateName => _currentStateName;

    //float _lastAttackTime = -100f;

    public GameObject RushAttackTrigger;
    public bool isDamaged;

    [HideInInspector] public int projectileFiredCount;
    [HideInInspector] public float projectileIntervalTimer;

    [HideInInspector] public bool isSpawnedMite;


    private void Awake()
    {
        Status = Instantiate(StatusOrigin);
    }

    private void Start()
    {
        Animation = GetComponent<EnemyAnimation>();
        // Null체크 해야함.
        Player = GameObject.FindWithTag("Player")?.transform;

        if (RushAttackTrigger != null)
        {
            RushAttackTrigger.SetActive(false);
        }
    }

    private void Update()
    {
        // 추후, 공격 중 피격상태 등 커버를 위해 병렬 FSM 구현해야함.
        // 현재 상태에서는, 0번에 Idle, None->Soft->Hard 순서로 할당할 것.

        // 0. 행동 미할당 예외 처리. 추후 안정화 작업 후 삭제 예정
        if (Behaviours == null || Behaviours.Count == 0)
        {
            Debug.LogWarning("No behaviour assigned");
            return;
        }

        // Die State
        if (_currentStateName == "Die")
        {
            return;
        }


        int idx = Behaviours.FindIndex(b => b.stateName == _currentStateName);
        var current = Behaviours[idx];

        // 0. 현재 조건이 Hard일 경우, 상태가 전이되기 전까지 계속해서 수행
        if (current.interruptType == InterruptType.Hard)
        {
            current.elapsedTime += Time.deltaTime;
            if (current.elapsedTime <= current.duration)
            {
                Debug.Log($"Act: {current.stateName}");
                current.action?.Act(this);
            }
            else
            {
                current.ResetTimer();
                _currentStateName = current.nextStateName;
                PlayAnimationOnce(Behaviours.Find(b => b.stateName == _currentStateName)?.animationName ?? "");
            }
            // Hard Type Interrupt 수행 중에는 이후 검사 수행하지 않음
            return;
        }


        // 1. Hard: 강제 인터럽트 조건 검사
        // ex) 죽음, 특수공격 등
        int hardIdx = Behaviours.FindIndex(
                b => b.interruptType == InterruptType.Hard &&
                     b.condition.IsMet(this));

        if (hardIdx >= 0 && Behaviours[hardIdx].stateName != _currentStateName)
        {
            _currentStateName = Behaviours[hardIdx].stateName;
            Behaviours[hardIdx].ResetTimer();
            PlayAnimationOnce(Behaviours[hardIdx].animationName);
        }


        // 2. Soft: 우선순위 인터럽트 순서대로 검사
        // ex) 공격, 소환 등 => 특정 상황에서는 hard 일 수도 있음. 
        int softIdx = -1;
        if (current.interruptType != InterruptType.Soft)
        {
            softIdx = Behaviours.FindIndex(
                    b => b.interruptType == InterruptType.Soft &&
                         b.condition.IsMet(this));
        }

        if (softIdx >= 0 && Behaviours[softIdx].stateName != _currentStateName)
        {
            _currentStateName = Behaviours[softIdx].stateName;
            Behaviours[softIdx].ResetTimer();
            PlayAnimationOnce(Behaviours[softIdx].animationName);
        }

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
            current.ResetTimer();
            _currentStateName = current.nextStateName;
            PlayAnimationOnce(Behaviours.Find(b => b.stateName == _currentStateName)?.animationName ?? "");
        }
    }


    // 애니메이션 중복 재생 방지용 함수
    private void PlayAnimationOnce(string animName)
    {
        if (_currentAnimation == animName) return;
        Animation.Play(animName);
        _currentAnimation = animName;
    }

    // 상태 강제 전이 함수 (ex, 돌진 중 충돌 시 돌진 중지)
    public void ForceState(string stateName)
    {
        int idx = Behaviours.FindIndex(b => b.stateName == stateName);
        if (idx >= 0)
        {
            _currentStateName = stateName;
            Behaviours[idx].ResetTimer();
            PlayAnimationOnce(Behaviours[idx].animationName);
        }
    }

    public void ForceToNextState()
    {
        int idx = Behaviours.FindIndex(b => b.stateName == _currentStateName);

        if (idx >= 0)
        {
            string next = Behaviours[idx].nextStateName;
            int nextIdx = Behaviours.FindIndex(b => b.stateName == next);

            if (nextIdx >= 0)
            {
                Behaviours[nextIdx].ResetTimer();
                _currentStateName = next;
                PlayAnimationOnce(Behaviours[nextIdx].animationName);
            }
        }
    }
}
