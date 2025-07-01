using UnityEngine;

public class MagicBullet_YSJ : MagicRoot_YSJ
{
    Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        FlyingAction?.Invoke(ownerArtifact, gameObject);

        rb2d.linearVelocity = transform.right * Speed;
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
