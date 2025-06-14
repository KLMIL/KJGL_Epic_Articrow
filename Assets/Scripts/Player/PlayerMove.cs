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

        #region [Move]
        public void Move()
        {
            if ((_rigid == null) || (_playerAnimator == null))
            {
                Debug.LogError("RigidBody2D or PlayerAnimator is null");
                return;
            }

            if (Managers.Input.MoveInput == Vector2.zero)
            {
                _rigid.linearVelocity *= _dampScale;
                _playerAnimator.CurrentState = PlayerAnimator.State.Idle;
            }
            else
            {
                Vector2 curDir = _rigid.linearVelocity;

                if ((curDir.sqrMagnitude < (_moveSpeed * _moveSpeed)) || (Managers.Input.MoveInput != curDir.normalized))
                {
                    Vector2 moveDir = Managers.Input.MoveInput * _moveSpeed;
                    _rigid.linearVelocity += (moveDir - curDir);
                    _playerAnimator.CurrentState |= PlayerAnimator.State.Walk;
                }
            }
        }
        #endregion
    }
}