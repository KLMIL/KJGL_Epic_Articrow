using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyStatusSO Status;
    public Transform Player;
    public EnemyAnimation Animation;

    public List<EnemyBehaviourUnit> Behaviours;

    int _currentStateIndex = 0;


    private void Awake()
    {
        Behaviours = new List<EnemyBehaviourUnit>()
        {
            new EnemyBehaviourUnit(new PlayerInSightCondition(), new ChasePlayerAction(), 1, "Chase"),
            new EnemyBehaviourUnit(new PlayerNotInSightCondition(), new RandomMoveAction(), 2, "RandomMove"),
            new EnemyBehaviourUnit(new AlwaysTrueCondition(), new IdleAction(), 0, "Idle")
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

        var current = Behaviours[_currentStateIndex];

#error // 이부분에서 지속시간 넣어서 각 행동별 시간 정해줘야함
        if (current.condition.IsMet(this))
        {
            current.action.Act(this);
            _currentStateIndex = current.nextStateIndex;
        }
    }
}
