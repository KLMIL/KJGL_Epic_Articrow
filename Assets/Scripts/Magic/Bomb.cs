using UnityEngine;

public class Bomb : Magic
{
    [SerializeField] float damage = 1f;
    [SerializeField] float range = 99f;
    [SerializeField] float speed = 2f;

    Rigidbody2D rb2d;
    Vector2 startPosition;
    public float BoomTimer;
    public GameObject BoomEffect;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        BoomEffect = Resources.Load<GameObject>("Magic/BombBoomEffect");
    }
    private void Start()
    {
        startPosition = transform.position;
        rb2d.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
        rb2d.AddTorque(Random.Range(-1f, 1f), ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, startPosition) > range)
        {
            Destroy(gameObject);
        }

        if (BoomTimer < 0)
        {
            Boom();
        }
        else 
        {
            BoomTimer -= Time.deltaTime;
        }
    }

    void Boom() 
    {
        Instantiate(BoomEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Health>(out Health targetHealth) && !collision.collider.GetComponent<MagicHand>())
        {
            targetHealth.TakeDamage(damage);
            Boom();
        }
    }
}
