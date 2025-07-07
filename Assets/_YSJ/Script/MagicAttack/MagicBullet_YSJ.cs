using UnityEngine;

public class MagicBullet_YSJ : MagicRoot_YSJ
{
    Rigidbody2D rb2d;
    LayerMask _obstacleLayerMask;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        _obstacleLayerMask = LayerMask.GetMask("Obstacle");
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
            CheckDestroy();
        }

        if (((1 << collision.gameObject.layer) & _obstacleLayerMask.value) != 0)
        {
            DestroyCount--;
            CheckDestroy();
        }
    }
}
