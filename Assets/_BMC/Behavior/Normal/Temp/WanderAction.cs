using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using UnityEngine.UIElements;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Wander", story: "[Self] Navigate To WanderPosition", category: "Action", id: "189ebf56c1aaeedd1c05bed1de007d45")]
public partial class WanderAction : Action
{
    // 추가로 변수가 더 필요하면 직접 타이핑해서 추가
    // BlackboardVariable<>로 선언하면 Behavior Graph의 인스펙터에 노출 가능

    [SerializeReference] public BlackboardVariable<GameObject> Self;

    NavMeshAgent agent;
    Vector3 wanderPosition;
    float currentWanderTime = 0f;
    float maxWanderTime = 5f;

    // 해당 노드가 실행될 때 1회 호출
    protected override Status OnStart()
    {
        int jitterMin = 0;
        int jitterMax = 360;
        float wanderRadius = UnityEngine.Random.Range(2.5f, 6f);
        int wanderJitter = UnityEngine.Random.Range(jitterMin, jitterMax);

        // BlackboardVariable<> 타입의 변수는 변수명.Value로 값을 설정하거나 불러올 수 있음
        wanderPosition = Self.Value.transform.position + Util.GetPositionFromAngle(wanderRadius, wanderJitter);
        agent = Self.Value.GetComponent<NavMeshAgent>();
        agent.SetDestination(wanderPosition);
        currentWanderTime = Time.time;

        return Status.Running;
    }

    // 해당 노드를 실행하는 동안 매프레임 호출
    protected override Status OnUpdate()
    {

        if((wanderPosition - Self.Value.transform.position).sqrMagnitude < 0.1f
            || Time.time - currentWanderTime > maxWanderTime) // 경로가 막혀있거나 어떤 이유로 목표까지 도달하지 못할 수도 있기 때문에 최대 재생 시간 설정
        {
            return Status.Success;  // 완료를 반환해 해당 노드 종료
        }
        return Status.Running;
    }

    // 해당 노드를 종료할 때 1회 호출
    protected override void OnEnd()
    {
    }
}