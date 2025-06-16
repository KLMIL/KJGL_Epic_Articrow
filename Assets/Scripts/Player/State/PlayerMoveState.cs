using System.Collections;
using UnityEngine;
using YSJ;

namespace BMC
{
    public class PlayerMoveState : State
    {
        PlayerFSM _playerFSM;
        Animator _anim;
        Rigidbody2D _rb;
        CheckPlayerDirection _checkPlayerDirection;

        float _moveSpeed = 6f;
        float _dampScale = 0.5f;
        Vector2 _moveInput;

        void Start()
        {
            _anim = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            _playerFSM = GetComponent<PlayerFSM>();
            _checkPlayerDirection = GetComponent<CheckPlayerDirection>();
        }

        public override void OnStateEnter()
        {
            Debug.Log("Move 진입");
        }

        public override void OnStateUpdate()
        {
            _moveInput = Managers.Input.MoveInput;
            _playerFSM.CheckAndSetAttack();
            _playerFSM.CheckAndSetDash();
            _playerFSM.CheckAndSetIdle();
            PlayMoveAnimation();
            Move();
        }

        public override void OnStateExit()
        {
            Debug.Log("Move 종료");
        }

        // Move 로직
        public void Move()
        {
            Vector2 currentDirection = _rb.linearVelocity;

            if ((currentDirection.sqrMagnitude < (_moveSpeed * _moveSpeed)) || (Managers.Input.MoveInput != currentDirection.normalized))
            {
                Vector2 moveDir = Managers.Input.MoveInput * _moveSpeed;
                _rb.linearVelocity += (moveDir - currentDirection);
            }
        }

        // Move 애니메이션 재생
        public void PlayMoveAnimation()
        {
            _playerFSM.Flip();
            switch (_checkPlayerDirection.CurrentDirection)
            {
                case CheckPlayerDirection.Direction.down:
                    _anim.Play("Walk_Down");
                    break;
                case CheckPlayerDirection.Direction.up:
                    _anim.Play("Walk_Up");
                    break;
                case CheckPlayerDirection.Direction.right:
                    _anim.Play("Walk_Right");
                    break;
                case CheckPlayerDirection.Direction.left:
                    _anim.Play("Walk_Right");
                    break;
            }
        }
    }
}