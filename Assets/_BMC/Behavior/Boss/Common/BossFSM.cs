using Unity.Behavior;
using UnityEngine;

namespace BMC
{
    /// <summary>
    /// 보스가 가지는 기본적인 상태 머신
    /// </summary>
    public class BossFSM : MonoBehaviour
    {
        public Animator Anim { get; private set; }
        public Rigidbody2D RB { get; private set; }
        public BossStatus Status { get; private set; }  // 보스 상태 정보
        public BossHitBox HitBox { get; private set; }  // 보스 히트 박스
        public BossHurtBox HurtBox { get; private set; }  // 보스 히트 박스

        [SerializeField] protected BehaviorGraphAgent _behaviorGraphAgent;
        Transform _visual;
        [SerializeField] protected Transform _target;

        public virtual void Awake()
        {
            Debug.Log("BossFSM Awake");
            Init();
        }

        public virtual void Init()
        {
            Anim = GetComponent<Animator>();
            RB = GetComponent<Rigidbody2D>();
            Status = GetComponent<BossStatus>();
            HitBox = GetComponentInChildren<BossHitBox>();
            HurtBox = GetComponentInChildren<BossHurtBox>();
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            _visual = transform.Find("Visual");
        }

        // x 방향으로 비주얼 회전
        public void FlipX(float x)
        {
            float angle = (x >= 0) ? 0 : 180;
            _visual.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}