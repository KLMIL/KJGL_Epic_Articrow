using UnityEngine;
using UnityEngine.AI;

namespace BMC
{
    /// <summary>
    /// 1 스테이지 Golem 보스의 상태 머신
    /// </summary>
    public class GolemFSM : BossFSM
    {
        public NavMeshAgent NavMeshAgent { get; private set; }        // NavMesh 에이전트
        public ShokeWaveHitBox ShokeWaveHitBox { get; private set; }  // 쇼크 웨이브 히트 박스

        public override void Awake()
        {
            Debug.Log("GolemFSM Awake");
            Init();
        }

        public override void Init()
        {
            base.Init();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            NavMeshAgent.updateRotation = false;
            NavMeshAgent.updateUpAxis = false;

            ShokeWaveHitBox = GetComponentInChildren<ShokeWaveHitBox>();
        }

        void Start()
        {
            HitBox.Init(Status.Damage);
            HurtBox.Init(this);
            _target = PlayerManager.Instance.transform;
            _behaviorGraphAgent.SetVariableValue("BossFSM", this);
            _behaviorGraphAgent.SetVariableValue("GolemFSM", this);
            _behaviorGraphAgent.SetVariableValue("Target", _target.gameObject);
        }
    }
}