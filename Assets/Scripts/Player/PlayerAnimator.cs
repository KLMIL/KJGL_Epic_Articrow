using BMC;
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

            if ((CurrentState & State.Attack) == State.Attack)
                return;

            //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            //if (stateInfo.IsName("Attack_Down2") ||
            //   stateInfo.IsName("Attack_Up2") ||
            //   stateInfo.IsName("Attack_Side2"))
            //{
            //    return; // 공격 애니메이션이 재생 중이면 다른 애니메이션을 재생하지 않음
            //}

            //// Attack상태일때
            //if ((CurrentState & State.Attack) == State.Attack)
            //{
            //    int step = PlayerManager.Instance.PlayerAttack.CurrentAttackStep;
            //    switch (checkDirection.CurrentDirection)
            //    {
            //        case CheckPlayerDirection.Direction.down:
            //            animator.Play($"Attack_Down{step}");
            //            spriteRenderer.flipX = false;
            //            break;
            //        case CheckPlayerDirection.Direction.up:
            //            animator.Play($"Attack_Up{step}");
            //            spriteRenderer.flipX = false;
            //            break;
            //        case CheckPlayerDirection.Direction.right:
            //            animator.Play($"Attack_Side{step}");
            //            spriteRenderer.flipX = false;
            //            break;
            //        case CheckPlayerDirection.Direction.left:
            //            animator.Play($"Attack_Side{step}");
            //            spriteRenderer.flipX = true;
            //            break;
            //    }
            //    return;
            //}

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