using UnityEngine;

namespace YSJ
{
    public class PlayerMove : MonoBehaviour
    {
        Rigidbody2D _rb;
        PlayerStatus _playerStatus;

        float _moveSpeed = 6f;
        float _dampScale = 0.5f;

        void Start ()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerStatus = GetComponentInChildren<PlayerStatus>();
        }

        public void Move()
        {
            if (Managers.Input.MoveInput == Vector2.zero)
            {
                _rb.linearVelocity *= _dampScale;
                _playerStatus.CurrentState &= ~PlayerStatus.PlayerState.Move;
            }
            else
            {
                Vector2 curDir = _rb.linearVelocity;

                if ((curDir.sqrMagnitude < (_moveSpeed * _moveSpeed)) || (Managers.Input.MoveInput != curDir.normalized))
                {
                    Vector2 moveDir = Managers.Input.MoveInput * _moveSpeed;
                    _rb.linearVelocity += (moveDir - curDir);
                    _playerStatus.CurrentState |= PlayerStatus.PlayerState.Move;
                }
            }
        }

        public void Stop()
        {
            _rb.linearVelocity = Vector2.zero;
            _playerStatus.CurrentState &= ~PlayerStatus.PlayerState.Move;
        }
    }
}