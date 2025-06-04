using UnityEngine;

public class Artifact_Animator : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        animator.Play("Idle");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0)) 
        {
            PlayAttack();
        }
    }

    void AnimationEnd() 
    {
        PlayIdle();
    }

    public void PlayAttack() 
    {
        animator.Play("Attack", -1, 0);
    }

    public void PlayIdle() 
    {
        animator.Play("Idle");
    }
}
