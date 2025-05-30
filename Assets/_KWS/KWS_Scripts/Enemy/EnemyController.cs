using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyStatusSO Status;
    public Transform Player;
    public EnemyAnimation Animation;

    public List<EnemyBehaviourUnit> Behaviours;

    int _previousStateIndex = 0;
    int _currentStateIndex = 0;


    private void Awake()
    {
        Behaviours = new List<EnemyBehaviourUnit>()
        {
            new EnemyBehaviourUnit(new PlayerInSightCondition(), new ChasePlayerAction(), 1, name: "Chase", duration: 2f),
            new EnemyBehaviourUnit(new PlayerNotInSightCondition(), new RandomMoveAction(), 2, name: "RandomMove", duration: 3.5f),
            new EnemyBehaviourUnit(new AlwaysTrueCondition(), new IdleAction(), 0, name: "Idle", duration: 1.5f)
        };
    }

    private void Start()
    {
        Status = Resources.Load<EnemyStatusSO>($"EnemyStatus/{gameObject.name}Status");
        Animation = GetComponent<EnemyAnimation>();
        Player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (Behaviours == null || Behaviours.Count == 0)
        {
            Debug.LogWarning("No behaviour assigned");
            return;
        }

        // 가장 먼저 조건이 true인 Behaviour 탐색
        int nextIndex = Behaviours.FindIndex(b => b.condition.IsMet(this));
        if (nextIndex < 0)
        {
            nextIndex = Behaviours.FindIndex(b => b.condition is AlwaysTrueCondition);
        }

        // 상태 전환이 발생하면 타이머 리셋 및 애니메이션 업데이트
        if (nextIndex != _currentStateIndex)
        {
            _previousStateIndex = _currentStateIndex;
            _currentStateIndex = nextIndex;
            Behaviours[_currentStateIndex].ResetTimer();

            PlayAnimationOnce(Behaviours[_currentStateIndex].name);
        }

        var current = Behaviours[_currentStateIndex];
        current.elapsedTime += Time.deltaTime;

        if (current.elapsedTime <= current.duration)
        {
            current.action.Act(this);
        }
        else
        {
            // duration을 다 사용하면 다음 상태로 강제 전환
            current.ResetTimer();
            _currentStateIndex = current.nextStateIndex;
        }
    }

    // 애니메이션 중복 재생 방지용 함수
    private string _currentAnimation = "";
    private void PlayAnimationOnce(string animName)
    {
        if (_currentAnimation == animName) return;
        Animation.Play(animName);
        _currentAnimation = animName;
    }
}
