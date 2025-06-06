using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Unity.Behavior;

public class EnemyFSM : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent navMeshAgent;
    private BehaviorGraphAgent behaviorAgent;

    public void Setup(Transform target, GameObject[] wayPoints)
    {
        this.target = target;

        navMeshAgent = GetComponent<NavMeshAgent>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        navMeshAgent.updateRotation = false; // 회전인 자동으로 설정되면 3D인 xz 평면 기준으로 에이전트가 회전하기 때문에 회전이 자동으로 되지 않도록 하기
        navMeshAgent.updateUpAxis = false;

        behaviorAgent.SetVariableValue("PatrolPoints", wayPoints.ToList()); // BehaviorGraphAgent의 PatrolPoints 변수에 WayPoints를 설정
    }
}