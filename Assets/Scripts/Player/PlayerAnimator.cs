using UnityEngine;

namespace YSJ
{
    public class PlayerAnimator : MonoBehaviour
    {
        public enum State
        {
            Idle = 0,
            Walk = 1 << 1,
            Attack = 1 << 2,
            Stun = 1 << 3
        }

        public State CurrentState { get; set; }
        CheckPlayerDirection checkDirection;
        Animator animator;
        SpriteRenderer spriteRenderer;

        private void Awake()
        {
            checkDirection = GetComponent<CheckPlayerDirection>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            PlayAnimation();
        }

        public void PlayAnimation()
        {
            if (!checkDirection)
            {
                Debug.LogError("CheckDirection 컴포넌트 없음!");
                enabled = false;
                return;
            }

            // 걷는상태일때
            if ((CurrentState & State.Walk) == State.Walk)
            {
                switch (checkDirection.CurrentDirection)
                {
                    case CheckPlayerDirection.Direction.down:
                        animator.Play("Walk_Down");
                        spriteRenderer.flipX = false;
                        break;
                    case CheckPlayerDirection.Direction.up:
                        animator.Play("Walk_Up");
                        spriteRenderer.flipX = false;
                        break;
                    case CheckPlayerDirection.Direction.right:
                        animator.Play("Walk_Right");
                        spriteRenderer.flipX = false;
                        break;
                    case CheckPlayerDirection.Direction.left:
                        animator.Play("Walk_Right");
                        spriteRenderer.flipX = true;
                        break;
                }
                return;
            }

            // Idle상태일때
            if ((CurrentState & State.Idle) == State.Idle)
            {
                switch (checkDirection.CurrentDirection)
                {
                    case CheckPlayerDirection.Direction.down:
                        animator.Play("Idle_Down");
                        spriteRenderer.flipX = false;
                        break;
                    case CheckPlayerDirection.Direction.up:
                        animator.Play("Idle_Up");
                        spriteRenderer.flipX = false;
                        break;
                    case CheckPlayerDirection.Direction.right:
                        animator.Play("Idle_Right");
                        spriteRenderer.flipX = false;
                        break;
                    case CheckPlayerDirection.Direction.left:
                        animator.Play("Idle_Right");
                        spriteRenderer.flipX = true;
                        break;
                }
                return;
            }
        }
    }
}