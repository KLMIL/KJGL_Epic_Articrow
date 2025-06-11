using UnityEngine;

public class PlayAnimation_YSJ : MonoBehaviour
{
    public enum animationClips 
    {
        Idle,
        Move,
        Ready,
        Attack,
        Hit,
        Dead,
    }
    public animationClips currentState = animationClips.Idle;

    Animator animator;

    protected void Init() 
    {
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        Init();
    }

    protected void PlayAnimation() 
    {
        switch (currentState) 
        {
            case animationClips.Idle:
                animator.Play("Idle");
                break;
            case animationClips.Move:
                animator.Play("Move");
                break;
            case animationClips.Ready:
                animator.Play("Ready");
                break;
            case animationClips.Attack:
                animator.Play("Attack");
                break;
            case animationClips.Hit:
                animator.Play("Hit");
                break;
            case animationClips.Dead:
                animator.Play("Dead");
                break;
        }
    }

    void Update()
    {
        PlayAnimation();
    }
}
