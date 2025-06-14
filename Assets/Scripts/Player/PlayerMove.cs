using UnityEngine;

namespace YSJ
{
    public class PlayerMove : MonoBehaviour
    {
        Rigidbody2D _rigid;
        PlayerAnimator _playerAnimator;

        float _moveSpeed = 6f;
        float _dampScale = 0.5f;

        void Start ()
        {
            _rigid = GetComponent<Rigidbody2D>();
            _playerAnimator = GetComponentInChildren<PlayerAnimator>();
        }

        private void FixedUpdate()
        {
            Move(Managers.Input.MoveInput, _moveSpeed);
        }

        #region [Move]
        void Move(Vector2 moveInput, float moveSpeed)
        {
            if ((_rigid == null) || (_playerAnimator == null))
            {
                Debug.LogError("RigidBody2D or PlayerAnimator is null");
                return;
            }

            if (moveInput == Vector2.zero)
            {
                _rigid.linearVelocity *= _dampScale;
                _playerAnimator.CurrentState |= PlayerAnimator.State.Idle;
            }
            else
            {
                Vector2 curDir = _rigid.linearVelocity;

                if ((curDir.sqrMagnitude < (moveSpeed * moveSpeed)) || (moveInput != curDir.normalized))
                {
                    Vector2 moveDir = moveInput * moveSpeed;
                    _rigid.linearVelocity += (moveDir - curDir);
                    _playerAnimator.CurrentState |= PlayerAnimator.State.Walk;
                }
            }
        }
        #endregion
    }
}