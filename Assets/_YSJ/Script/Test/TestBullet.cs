using UnityEngine;

public class TestBullet : MagicRoot_YSJ
{
    Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb2d.linearVelocity = transform.right * Speed;

        FlyingAction?.Invoke(ownerArtifact, gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamagable>() != null)
        {
            OnHit(collision);
            collision.GetComponent<IDamagable>().TakeDamage(AttackPower);
        }
    }
}
