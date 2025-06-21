using Unity.Behavior;
using UnityEngine;

namespace BMC
{
    public class BossFSM : MonoBehaviour
    {
        Transform _visual;
        public Rigidbody2D RB { get; private set; }
        public Animator Anim { get; private set; }
        public BossStatus Status { get; private set; }  // 보스 상태 정보
        public BossHitBox HitBox { get; private set; }  // 보스 히트 박스

        [SerializeField] BehaviorGraphAgent _behaviorGraphAgent;
        [SerializeField] Transform _target;

        [Header("돌진")]
        bool _isReflect = false;
        public Vector2 RushDirection { get; set; } // 돌진 방향
        [SerializeField] LayerMask _stopLayerMask;

        void Awake()
        {
            Init();
        }

        void Init()
        {
            Anim = GetComponent<Animator>();
            RB = GetComponent<Rigidbody2D>();
            Status = GetComponent<BossStatus>();
            HitBox = GetComponentInChildren<BossHitBox>();
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            _visual = transform.Find("Visual");
            //_stopLayerMask = LayerMask.GetMask("Obstacle");
        }

        void Start()
        {
            HitBox.Init(Status.Damage);
            _target = GameObject.FindWithTag("Player").transform;
            _behaviorGraphAgent.SetVariableValue("Target", _target.gameObject);
        }

        // x 방향으로 비주얼 회전
        public void FlipX(float x)
        {
            float angle = (x>=0) ? 0 : 180;
            _visual.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}