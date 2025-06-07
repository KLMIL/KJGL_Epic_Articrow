using UnityEngine;

namespace YSJ
{
    [System.Serializable]
    public class PlayerMove
    {
        float _moveSpeed = 7f;
        float _damping = 0.5f;

        public void Move(Vector2 inputValue, Rigidbody2D rigid, PlayerAnimator animator)
        {
            if ((rigid == null) || (animator == null))
            {
                Debug.LogError("RigidBody2D or PlayerAnimator is null");
                return;
            }
            
            if (inputValue != Vector2.zero)
            {
                rigid.linearVelocity = inputValue * _moveSpeed;
                animator.currentState |= PlayerAnimator.State.Walk;
            }
            else
            {
                rigid.linearVelocity *= _damping;
                animator.currentState &= ~PlayerAnimator.State.Walk;
            }
        }
    }
}