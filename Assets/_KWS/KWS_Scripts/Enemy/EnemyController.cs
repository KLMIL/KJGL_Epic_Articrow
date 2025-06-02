using System.Collections.Generic;
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

    int _currentStateIndex = 0;
    string _currentAnimation = "";

    float _lastAttackTime = -100f;

    public GameObject RushAttackTrigger;

    public bool isDamaged;


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
        if (Input.GetMouseButtonDown(1))
        {
            GetComponent<EnemyTakeDamage>().TakeDamage(5);
        }

        // 추후, 공격 중 피격상태 등 커버를 위해 병렬 FSM 구현해야함.
        // 현재 상태에서는, 0번에 Idle, None->Soft->Hard 순서로 할당할 것.

        // 0. 행동 미할당 예외 처리. 추후 안정화 작업 후 삭제 예정
        if (Behaviours == null || Behaviours.Count == 0)
        {
            Debug.LogWarning("No behaviour assigned");
            return;
        }

        // Die State
        if (_currentStateIndex == -1)
        {
            return;
        }


        // 1. Hard: 강제 인터럽트 조건 검사
        // ex) 죽음, 피격 등 => 특정 상황에서는 soft 일 수도 있음.
        int hardIdx = Behaviours.FindIndex(
            b => b.interruptType == InterruptType.Hard &&
                 b.condition.IsMet(this));

        if (hardIdx >= 0 && hardIdx != _currentStateIndex)
        {
            _currentStateIndex = hardIdx;
            Behaviours[_currentStateIndex].ResetTimer();
            PlayAnimationOnce(Behaviours[_currentStateIndex].animationName);
            Behaviours[_currentStateIndex].action?.Act(this);   

            // 현재 행동 완료하기 위해 return
            return;
        }


        // 2. Soft: 우선순위 인터럽트 순서대로 검사
        // ex) 공격, 소환 등 => 특정 상황에서는 hard 일 수도 있음. 
        int softIdx = -1;
        if (Behaviours[_currentStateIndex].interruptType != InterruptType.Soft)
        {
            softIdx = Behaviours.FindIndex(
            b => b.interruptType == InterruptType.Soft &&
                 b.condition.IsMet(this));
        }

        if (softIdx >= 0 && softIdx != _currentStateIndex)
        {
            _currentStateIndex = softIdx;
            Behaviours[_currentStateIndex].ResetTimer();
            PlayAnimationOnce(Behaviours[_currentStateIndex].animationName);

            // 일반 루프도 실행
        }


        // 3. None: 기본 루프 수행
        // Idle, RandomMove 등
        var current = Behaviours[_currentStateIndex];
        current.elapsedTime += Time.deltaTime;

        if (current.elapsedTime <= current.duration)
        {
            current.action?.Act(this);
        }
        else
        {
            current.ResetTimer();
            _currentStateIndex = current.nextStateIndex;
            PlayAnimationOnce(Behaviours[_currentStateIndex].animationName);
        }
    }


    // 애니메이션 중복 재생 방지용 함수
    private void PlayAnimationOnce(string animName)
    {
        if (_currentAnimation == animName) return;
        Animation.Play(animName);
        _currentAnimation = animName;
    }

}
