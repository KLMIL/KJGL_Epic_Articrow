using UnityEngine;

public class MagicBullet : Magic
{
    [SerializeField] float damage = 10f;
    [SerializeField] float range = 5f;
    [SerializeField] float speed = 20f;
    [SerializeField] float pushForce = 10f;

    Rigidbody2D rb2d;
    Vector2 startPosition;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }
    private void Start()
    {
        startPosition = transform.position;
        rb2d.linearVelocity = direction.normalized * speed;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, startPosition) > range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 나 제외
        if (collision.GetComponent<MagicHand>()) 
        {
            return;
        }

        // 체력이 있는 애를 만나면
        if (collision.TryGetComponent<Health>(out Health targetHealth))
        {
            A_CollisionEvent?.Invoke(collision);
            targetHealth.TakeDamage(damage);
            if(collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D targetRb2d))
            {
                targetRb2d.AddForce((targetRb2d.position - (Vector2)transform.position).normalized * pushForce);
            }
        }

        // 마법이 아닌 충돌체 만나면 나 파괴
        if (!collision.GetComponent<Magic>() || collision.GetComponent<Barrier>()) 
        {
            Destroy(gameObject);
        }
    }
}
