using UnityEngine;

public class MagicFluteNormalAttack_YSJ : MagicRoot_YSJ
{
    float AttackSize = 1f;

    SpriteRenderer spriteRenderer;
    Color spriteColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteColor = GetComponent<SpriteRenderer>().color;
    }

    private void Start()
    {
        AttackSize = transform.localScale.x;
    }

    private void Update()
    {
        FlyingAction?.Invoke(ownerArtifact, gameObject);

        transform.localScale += Vector3.one * Speed * Time.deltaTime * AttackSize;

        spriteColor.a = Mathf.Lerp(1, 0, elapsedTime/LifeTime);
        foreach (Transform child in transform)
        {
            Color color = child.GetComponent<SpriteRenderer>().color;
            color.a = Mathf.Lerp(1, 0, elapsedTime / LifeTime);
            child.GetComponent<SpriteRenderer>().color = color;
        }
        spriteRenderer.color = spriteColor;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamagable>() != null)
        {
            OnHit(collision);
            collision.GetComponent<IDamagable>().TakeDamage(AttackPower);
            //CheckDestroy();
        }
    }
}
