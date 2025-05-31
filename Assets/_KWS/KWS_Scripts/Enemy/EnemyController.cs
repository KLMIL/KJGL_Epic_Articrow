using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Status: ScriptableObject")]
    public EnemyStatusSO Status;
    [Header("Player: Find by Tag")]
    public Transform Player;
    [Header("Animation Handler: GetComponent")]
    public EnemyAnimation Animation;

    [Space(10)]
    [Header("Behaviour List Assign on Inspector")]
    public List<EnemyBehaviourUnit> Behaviours;

    int _currentStateIndex = 0;
    string _currentAnimation = "";
    

    private void Start()
    {
        // 현재는 Resource에서 불러오는 방식으로 구현했는데, Inspector 할당 방식으로 변경해도 됨
        //Status = Resources.Load<EnemyStatusSO>($"EnemyStatus/{gameObject.name}Status");

        Animation = GetComponent<EnemyAnimation>();
        Player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        // 할당된 행동이 없다면 아무것도 하지 않음. 추후 안정화 이후 검사 제거 예정
        if (Behaviours == null || Behaviours.Count == 0)
        {
            Debug.LogWarning("No behaviour assigned");
            return;
        }

        // Inspector에서 할당된 순서대로 조건 검사해서 우선순위 높은 행동 찾기
        int nextStateIndex = Behaviours.FindIndex(b => b.condition != null && b.condition.IsMet(this));

        // 만족하는 조건이 없다면 Idle(Always true condition) 실행
        if (nextStateIndex < 0)
        {
            nextStateIndex = Behaviours.FindIndex(b => b.condition is AlwaysTrueConditionSO);
            if (nextStateIndex < 0)
            {
                nextStateIndex = 0;
            }
        }

        // 상태 전환이 일어났다면 타이머 리셋 및 애니메이션 재생
        if (nextStateIndex != _currentStateIndex)
        {
            _currentStateIndex = nextStateIndex;
            Behaviours[_currentStateIndex].ResetTimer();

            PlayAnimationOnce(Behaviours[_currentStateIndex].name);
        }

        var current = Behaviours[_currentStateIndex];
        current.elapsedTime += Time.deltaTime;

        // Duration 안에서 같은 애니메이션 반복
        if (current.elapsedTime <= current.duration)
        {
            current.action?.Act(this);
        }
        else
        {
            // duration을 다 사용하면 다음 상태로 강제 전환
            current.ResetTimer();
            _currentStateIndex = current.nextStateIndex;
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
