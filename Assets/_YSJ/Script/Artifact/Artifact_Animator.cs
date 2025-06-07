using UnityEngine;

public class Artifact_Animator : MonoBehaviour
{
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    void Start()
    {
        _animator.Play("Idle");
    }

    void AnimationEnd() 
    {
        _animator.Play("Idle");
    }
}
