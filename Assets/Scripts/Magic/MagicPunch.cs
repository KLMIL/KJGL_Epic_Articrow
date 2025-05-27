using UnityEngine;

public class MagicPunch : Magic
{
    public float damage = 999f;
    public float pushForce = 10f;

    public float punchTime = 10f;
    public float rushForce = 5f;
    float elapsedTime = 0f;

    void Awake()
    {
        transform.position += (Vector3)direction;
    }

    private void Start()
    {
        transform.parent.GetComponent<Move>().enabled = false;
        if (transform.parent.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb2d))
        {
            rb2d.AddForce(((Vector2)transform.position - rb2d.position).normalized * rushForce, ForceMode2D.Impulse);
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > punchTime) 
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
            if (collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D targetRb2d))
            {
                targetRb2d.AddForce((targetRb2d.position - (Vector2)transform.position).normalized * pushForce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDestroy()
    {
        transform.parent.GetComponent<Move>().enabled = true;
    }
}
