using UnityEngine;

namespace YSJ
{
    [System.Serializable]
    public class PlayerMove
    {
        float _moveSpeed = 28f;
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
                //rigid.linearVelocity = inputValue * _moveSpeed;
                rigid.AddForce(inputValue * _moveSpeed, ForceMode2D.Force);
                animator.CurrentState |= PlayerAnimator.State.Walk;
            }
            else
            {
                //rigid.linearVelocity *= _damping;
                animator.CurrentState &= ~PlayerAnimator.State.Walk;
            }
        }
    }
}