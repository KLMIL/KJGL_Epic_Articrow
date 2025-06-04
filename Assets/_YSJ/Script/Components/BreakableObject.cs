using UnityEngine;
using YSJ;

public class BreakableObject : MonoBehaviour
{
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            animator.Play("Break");
        }
    }
}
