using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Play(string animationName)
    {
        animator.SetTrigger(animationName);
    }
}
