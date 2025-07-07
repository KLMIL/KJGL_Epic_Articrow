using UnityEngine;

public class MagicPunch_YSJ : MagicRoot_YSJ
{
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;

    Color color;
    float _defaultAlpha;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        color = spriteRenderer.color;
        _defaultAlpha = color.a;
    }

    private void Update()
    {
        FlyingAction?.Invoke(ownerArtifact, gameObject);

        rb2d.linearVelocity = transform.right * Speed;
        color.a = Mathf.Lerp(_defaultAlpha, 0, elapsedTime/LifeTime * 0.1f);
        spriteRenderer.color = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamagable>() != null)
        {
            OnHit(collision);
            collision.GetComponent<IDamagable>().TakeDamage(AttackPower);
            CheckDestroy();
        }
    }
}
