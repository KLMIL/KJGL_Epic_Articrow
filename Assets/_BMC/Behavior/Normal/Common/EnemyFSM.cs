using System.Linq;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

namespace BMC
{
    public class EnemyFSM : MonoBehaviour
    {
        Transform _visual;
        public Animator Anim { get; private set; }
        public Rigidbody2D RB { get; private set; }
        public EnemyStatus Status { get; private set; }
        public EnemyHurtBox HurtBox { get; private set; }  // 보스 히트 박스

        [SerializeField] BehaviorGraphAgent _behaviorGraphAgent;
        [SerializeField] Transform _target;

        void Awake()
        {
            Init();
        }

        void Init()
        {
            Anim = GetComponent<Animator>();
            RB = GetComponent<Rigidbody2D>();
            Status = GetComponent<EnemyStatus>();
            HurtBox = GetComponentInChildren<EnemyHurtBox>();
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            _visual = transform.Find("Visual");
            //_stopLayerMask = LayerMask.GetMask("Obstacle");
        }

        void Start()
        {
            Status.Init();
            HurtBox.Init(this);
            _target = PlayerManager.Instance.transform;
            _behaviorGraphAgent.SetVariableValue("Target", _target.gameObject);
        }

        // x 방향으로 비주얼 회전
        public void FlipX(float x)
        {
            float angle = (x >= 0) ? 0 : 180;
            _visual.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}