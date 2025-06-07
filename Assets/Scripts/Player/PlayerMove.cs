using UnityEngine;

namespace YSJ
{
    public class PlayerMove : MonoBehaviour
    {
        float _moveSpeed =5f;
        float _damping = 0.5f;
        Rigidbody2D _rb2d;

        void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
        }

        public void Move()
        {
            Vector2 inputValue = Managers.Input.MoveInput;

            if (inputValue != Vector2.zero)
            {
                _rb2d.linearVelocity = inputValue * _moveSpeed;
                if (TryGetComponent<PlayerAnimator_YSJ>(out PlayerAnimator_YSJ animator))
                {
                    animator.currentState |= PlayerAnimator_YSJ.State.Walk;
                }
            }
            else
            {
                _rb2d.linearVelocity *= _damping;
                if (TryGetComponent<PlayerAnimator_YSJ>(out PlayerAnimator_YSJ animator))
                {
                    animator.currentState &= ~PlayerAnimator_YSJ.State.Walk;
                }
            }
        }
    }
}