using UnityEngine;

namespace YSJ
{
    public class PlayerMove : MonoBehaviour
    {
        Rigidbody2D _rigid;
        PlayerAnimator _playerAnimator;

        float _moveSpeed = 5f;

        void Start ()
        {
            _rigid = GetComponent<Rigidbody2D>();
            _playerAnimator = GetComponent<PlayerAnimator>();
        }

        private void FixedUpdate()
        {
            Move(Managers.Input.MoveInput, _moveSpeed);
        }

        #region [Move]
        void Move(Vector2 inputValue, float moveSpeed)
        {
            if ((_rigid == null) || (_playerAnimator == null))
            {
                Debug.LogError("RigidBody2D or PlayerAnimator is null");
                return;
            }

            // 최종 이동속도
            Vector2 moveDir = inputValue * _moveSpeed;

            // 이동하기 전에 더 작은 속도를 뺄셈 (외력으로 인한 속도가 높아질 것을 고려)
            /*bool lowSpeed = (_rigid.linearVelocity.sqrMagnitude <= moveDir.sqrMagnitude);
            Vector2 reverseDir = lowSpeed ? _rigid.linearVelocity : moveDir;

            _rigid.linearVelocity -= reverseDir;
            _rigid.linearVelocity += moveDir;*/
            _rigid.MovePosition(_rigid.position + (moveDir * Time.fixedDeltaTime));
            _playerAnimator.CurrentState |= PlayerAnimator.State.Walk;
        }
        #endregion
    }
}