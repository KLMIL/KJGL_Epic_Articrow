using UnityEngine;
using YSJ;

namespace BMC
{
    public class PlayerIdleState : State
    {
        Animator _anim;
        Rigidbody2D _rb;
        PlayerFSM _playerFSM;
        CheckPlayerDirection _checkPlayerDirection;

        float _dampScale = 0.5f;

        void Awake()
        {
            _anim = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            _playerFSM = GetComponent<PlayerFSM>();
            _checkPlayerDirection = GetComponent<CheckPlayerDirection>();
        }

        public override void OnStateEnter()
        {
            Debug.Log("Idle 진입");
        }

        public override void OnStateUpdate()
        {
            Debug.Log("Idle 상태");
            _playerFSM.CheckAndSetAttack();
            _playerFSM.CheckAndSetMove();

            PlayIdleAnimation();
            if (Managers.Input.MoveInput == Vector2.zero)
            {
                _rb.linearVelocity *= _dampScale;
            }
        }

        public override void OnStateExit()
        {
            Debug.Log("Idle 종료");
        }

        // Idle 애니메이션 재생
        public void PlayIdleAnimation()
        {
            _playerFSM.Flip();
            switch (_checkPlayerDirection.CurrentDirection)
            {
                case CheckPlayerDirection.Direction.Down:
                    _anim.Play("Idle_Down");
                    break;
                case CheckPlayerDirection.Direction.Up:
                    _anim.Play("Idle_Up");
                    break;
                case CheckPlayerDirection.Direction.Right:
                    _anim.Play("Idle_Right");
                    break;
                case CheckPlayerDirection.Direction.Left:
                    _anim.Play("Idle_Right");
                    break;
            }
        }
    }
}