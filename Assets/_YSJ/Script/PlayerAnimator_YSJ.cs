using UnityEngine;

public class PlayerAnimator_YSJ : MonoBehaviour
{
    public enum State 
    {
        Idle = 0,
        Walk = 1 << 1,
        Attack = 1 << 2,
        Stun = 1 << 3
    }
    public State currentState;
    CheckDirection_YSJ checkDirection;
    Animator animator;

    private void Awake()
    {
        checkDirection = GetComponent<CheckDirection_YSJ>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayAnimation(currentState);
    }

    public void PlayAnimation(State newState)
    {
        if (!checkDirection)
        {
            Debug.LogError("CheckDirection 컴포넌트 없음!");
            this.enabled = false;
            return;
        }

        // 걷는상태일때
        if ((currentState & State.Walk) == State.Walk)
        {
            switch (checkDirection.CurrentDirection)
            {
                case CheckDirection_YSJ.Direction.down:
                    animator.Play("Walk_Down");
                    break;
                case CheckDirection_YSJ.Direction.up:
                    animator.Play("Walk_Up");
                    break;
                case CheckDirection_YSJ.Direction.right:
                    animator.Play("Walk_Right");
                    break;
                case CheckDirection_YSJ.Direction.left:
                    animator.Play("Walk_Left");
                    break;
            }
            return;
        }

        // Idle상태일때
        if ((currentState & State.Idle)== State.Idle)
        {
            switch (checkDirection.CurrentDirection)
            {
                case CheckDirection_YSJ.Direction.down:
                    animator.Play("Idle_Down");
                    break;
                case CheckDirection_YSJ.Direction.up:
                    animator.Play("Idle_Up");
                    break;
                case CheckDirection_YSJ.Direction.left:
                    animator.Play("Idle_Left");
                    break;
                case CheckDirection_YSJ.Direction.right:
                    animator.Play("Idle_Right");
                    break;
            }
            return;
        }
    }
}
