using UnityEngine;

public class Door_YSJ : MonoBehaviour
{
    Animator animator;
    public bool isOpened;
    bool isEnd;

    public float DestroyTime = 1f;
    float elapsedTime = 0f;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isOpened) 
        {
            animator.Play("Open");
        }

        if (isEnd) 
        {
            DecreaseAlpha();
        }
    }
    // 애니메이션에서 실행
    void OpenEnd() 
    {
        isEnd = true;
    }

    void DecreaseAlpha() 
    {
        elapsedTime += Time.deltaTime;
        Color c = GetComponent<SpriteRenderer>().color;
        c.a = Mathf.Lerp(1, 0, elapsedTime);
        GetComponent<SpriteRenderer>().color = c;
    }
}
