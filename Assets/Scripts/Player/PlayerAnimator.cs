using UnityEngine;

namespace YSJ
{
    public class PlayerAnimator : MonoBehaviour
    {
        public enum State
        {
            Idle = 0,
            Move = 1 << 1,
            Attack = 1 << 2,
            Stun = 1 << 3,
            Hurt = 1 << 4,
            Dead = 1 << 5,
        }

        public State CurrentState { get; set; }
        CheckPlayerDirection _checkPlayerDirection;
        Animator _anim;
        SpriteRenderer _spriteRenderer;

        void Awake()
        {
            _checkPlayerDirection = GetComponent<CheckPlayerDirection>();
            _anim = GetComponent<Animator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        void Start()
        {
            // 스테이터스에 액션 연결
            PlayerStatus.OnDeadAction += PlayDead;
        }

        void Update()
        {
            PlayAnimation();
        }

        public void PlayAnimation()
        {
            if (!_checkPlayerDirection || _checkPlayerDirection.CurrentDirection == CheckPlayerDirection.Direction.None)
            {
                return;
            }

            // Dead 상태
            if ((CurrentState & State.Dead) == State.Dead)
            {
                FlipX();
                _anim.Play("Dead");
                return;
            }

            if ((CurrentState & State.Attack) == State.Attack)
            {
                OnHurtAnimationEndEvent();
                return;
            }

            //Hurt 상태
            if ((CurrentState & State.Hurt) == State.Hurt)
            {
                FlipX();
                _anim.Play("Hurt");
                return;
            }

            // Move 상태
            if ((CurrentState & State.Move) == State.Move)
            {
                _anim.Play($"Move_{_checkPlayerDirection.CurrentDirection}");
                return;
            }

            // Idle 상태
            if ((CurrentState & State.Idle) == State.Idle)
            {
                _anim.Play($"Idle_{_checkPlayerDirection.CurrentDirection}");
                return;
            }
        }

        public void AttackAnimation(int step)
        {
            _anim.Play($"Attack_{_checkPlayerDirection.CurrentDirection}{step}");
        }

        void FlipX()
        {
            _spriteRenderer.flipX = (_checkPlayerDirection.CurrentDirection == CheckPlayerDirection.Direction.Left) ? true : false;
        }

        // Hurt 애니메이션이 끝났을 때 호출되는 애니메이션 이벤트
        void OnHurtAnimationEndEvent() 
        {
            CurrentState &= ~State.Hurt; // Hurt 상태 해제
        }

        void PlayDead()
        {
            CurrentState |= State.Dead; // Dead상태 추가
        }
    }
}