using UnityEngine;

public class TestBullet : MagicRoot_YSJ
{
    Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        magicType = MagicType.NormalAttack;
    }

    private void Update()
    {
        rb2d.linearVelocity = transform.right * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamagable>() != null)
        {
            collision.GetComponent<IDamagable>().TakeDamage(AttackPower);
        }
    }
}
