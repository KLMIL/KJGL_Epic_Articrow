using UnityEngine;

public class BombBoomEffect : Magic
{
    Vector2 effectScale;
    public float BoomTime = 0.5f;
    public float damage = 10f;
    float elapsedTime = 0f;
    float pushForce = 10f;

    private void OnEnable()
    {
        effectScale = transform.localScale;
        transform.localScale = Vector2.zero;
    }
    void Update()
    {
        if (elapsedTime < BoomTime)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, effectScale, elapsedTime / BoomTime);
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = Mathf.Lerp(1, 0, elapsedTime / BoomTime);
            GetComponent<SpriteRenderer>().color = color;
        }
        else 
        {
            Destroy(gameObject);
        }
        elapsedTime += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Health>(out Health targetHealth)) 
        {
            targetHealth.TakeDamage(damage);
        }
        if (collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb2d))
        {
           rb2d.AddForce((rb2d.position - (Vector2)transform.position) * pushForce, ForceMode2D.Impulse);
        }
    }
}
