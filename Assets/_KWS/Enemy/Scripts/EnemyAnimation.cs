using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    Animator animator;
    [HideInInspector] public string CurrentAnimation = "";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Play(string animationName)
    {
        animator.SetTrigger(animationName);
    }

    public void PlayAnimationOnce(string animName)
    {
        if (CurrentAnimation == animName) return;
        Play(animName);
        CurrentAnimation = animName;
    }
}
